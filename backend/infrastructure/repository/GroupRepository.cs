﻿using System.Data.SqlTypes;
using System.Security.Authentication;
using api.models;
using Dapper;
using infrastructure.dataModels;
using Npgsql;

namespace infrastructure.repository;

public class GroupRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public GroupRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public Group CreateGroup(Group group)
    {
        var sql =
            $@"
            insert into groups.group (name, description, image_url, created_date) 
            values (@Name, @Description, @ImageUrl, @CreatedDate) 
            returning *;
            ";

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.QueryFirst<Group>(sql,
                new { group.Name, group.Description, group.ImageUrl, group.CreatedDate });
        }
        catch (Exception e)
        {
            throw new SqlTypeException("Could not create group", e);
        }
    }

    public IEnumerable<Group> GetMyGroups(int userId)
    {
        var sql =
            $@"
            select * from groups.group_members
            join groups.group on groups.group_members.group_id = id 
            where groups.group_members.user_id = @userId;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<Group>(sql, new { userId });
        }
    }


    public bool AddUserToGroup(UserInGroupDto userInGroupDto)
    {
        var sql =
            $@"
            insert into groups.group_members (user_id, group_id, owner) 
            values (@UserId, @GroupId, @IsOwner);
            ";

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.Execute(sql, new
            {
                UserId = userInGroupDto.UserId,
                GroupId = userInGroupDto.GroupId,
                IsOwner = userInGroupDto.IsOwner
            }) == 1;
        }
        catch (Exception e)
        {
            throw new SqlTypeException("Could not add User to Group", e);
        }
    }

    public bool IsUserInGroup(int userId, int groupId)
    {
        var sql =
            $@"
            select * from groups.group_members 
            where user_id = @userId  
            and group_id = @groupId;
            ";

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.QuerySingleOrDefault(sql, new { userId, groupId }) != null;
        }
        catch (Exception e)
        {
            throw new AuthenticationException();
        }
    }

    public Group GetGroupById(int groupId)
    {
        var sql =
            $@"
            select 
                id as {nameof(Group.Id)},
                name as {nameof(Group.Name)},
                description as {nameof(Group.Description)},
                image_url as {nameof(Group.ImageUrl)},
                created_date as {nameof(Group.CreatedDate)}
            from groups.group 
            where id = @groupId;
            ";

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.QueryFirst<Group>(sql, new { groupId });
        }
        catch (Exception e)
        {
            throw new SqlTypeException("Could not read the group", e);
        }
    }

    public int IsUserGroupOwner(int groupId)
    {
        string sql = @"
               SELECT groups.group_members.user_id
               FROM groups.group_members
               WHERE groups.group_members.group_id = @groupId
               AND groups.group_members.owner = true;";
        
        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.QueryFirstOrDefault<int>(sql, new { groupId });
        }
        catch (Exception e)
        {
            throw new SqlTypeException("Failed to retrieve owner ID of the group");
        }
    }

    public IEnumerable<GroupInviteNotification> GetGroupInviteNotifications(int receiverId, DateTime lastUpdated)
    {


        string sql = $@"
SELECT
    groups.group.id AS GroupId,
    groups.group.name AS GroupName,
    groups.group.description AS GroupDescription,
    users.user.id AS SenderId,
    users.user.email AS SenderEmail,
    users.user.full_name AS SenderFullName,
    group_invitation.date_received AS InviteReceived
FROM groups.group_invitation
INNER JOIN groups.group
    ON group_invitation.group_id = groups.group.id
INNER JOIN users.user
    ON group_invitation.sender_id = users.user.id
WHERE group_invitation.receiver_id = @receiverId
    AND group_invitation.date_received > @lastUpdated";

        try
        {
            using (NpgsqlConnection conn = _dataSource.OpenConnection())
            {
                return conn.Query<GroupInviteNotification>(sql, new { receiverId, lastUpdated });
            }
        }
        catch (Exception e)
        {
            throw new SqlTypeException("Failed to retrieve group invitations", e);
        }
    }
    
    public bool InviteUserToGroup(FullGroupInvitation groupInvitation)
    {
        string sql = @"
                INSERT INTO groups.group_invitation
                (receiver_id, group_id, sender_id, date_received) 
                VALUES (@ReceiverId, @GroupId, @SenderId, @TimeNow);";
        
        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.Execute(sql, new
            {
                groupInvitation.ReceiverId,
                groupInvitation.GroupId,
                groupInvitation.SenderId,
                TimeNow =  DateTime.Now
            }) == 1;
        }
        catch (Exception e)
        {
            throw new SqlTypeException("Failed to invite user to group", e);
        }
    }

    public bool DeleteInvite(UserInGroupDto user)
    {
        string sql = $@"
                DELETE FROM groups.group_invitation
                WHERE group_invitation.receiver_id = @UserId
                AND group_invitation.group_id = @GroupId;";
        
        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.Execute(sql, new
            {
                user.UserId,
                user.GroupId
            }) == 1;
        }
        catch (Exception e)
        {
            throw new SqlTypeException("Failed to invite user to group", e);
        }
    }
}