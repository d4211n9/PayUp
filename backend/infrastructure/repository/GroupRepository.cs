using System.Data.SqlTypes;
using api.models;
using Dapper;
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
            values (@Name, @Description, @Image_Url, @Created_Date) 
            returning *;
            ";

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.QueryFirst<Group>(sql,
                new { group.Name, group.Description, group.Image_Url, group.Created_Date });
        }
        catch(Exception e)
        {
            throw new SqlTypeException("Could not create group", e);
        }

    }

    public bool AddUserToGroup(int userId, int groupId, bool isOwner)
    {
        var sql =
            $@"
            insert into groups.group_members (user_id, group_id, owner) 
            values (@userId, @groupId, @isOwner);
            ";

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.Execute(sql, new { userId, groupId, isOwner }) == 1;
        }
        catch(Exception e)
        {
            throw new SqlTypeException("Could not add User to Group", e);
        }
    }

    public int IsUserGroupOwner(int groupId)
    {
        string sql = @"
               SELECT groups.group_members.user_id
               FROM groups.group_members
               WHERE groups.group_members.group_id = @groupId
               AND groups.group_members.owner = true;";

        NpgsqlConnection conn = null;

        try
        {
            conn = _dataSource.OpenConnection();
            return conn.QueryFirstOrDefault<int>(sql, new { groupId });
        }
        catch (Exception e)
        {
            throw new SqlTypeException("Failed to retrieve owner ID of the group");
        }
        finally
        {
            if (conn != null) conn.Close();
        }
    }

    public bool IsUserInGroup(int userId, int groupId)
    {
        string sql = @"
                SELECT user_id
                FROM groups.group_members
                WHERE user_id = @userId
                AND group_id = @groupId;";

        NpgsqlConnection conn = null;

        try
        {
            conn = _dataSource.OpenConnection();
            return conn.QueryFirstOrDefault<int>(sql, new
            {
                userId,
                groupId
            }) == userId;
        }
        catch (Exception e)
        {
            throw new SqlTypeException("Failed to check if user is in group", e);
        }
        finally
        {
            if (conn != null) conn.Close();
        }
    }

    public bool InviteUserToGroup(FullGroupInvitation groupInvitation)
    {
        string sql = @"
                INSERT INTO groups.group_invitation
                (receiver_id, group_id, sender_id, date_received) 
                VALUES (@ReceiverId, @GroupId, @SenderId, @TimeNow);";

        DateTime timeNow = DateTime.Now;

        NpgsqlConnection conn = null;

        try
        {
            conn = _dataSource.OpenConnection();
            return conn.Execute(sql, new
            {
                groupInvitation.ReceiverId,
                groupInvitation.GroupId,
                groupInvitation.SenderId,
                TimeNow = timeNow
            }) == 1;
        }
        catch (Exception e)
        {
            throw new SqlTypeException("Failed to invite user to group", e);
        }
        finally
        {
            if (conn != null) conn.Close();
        }
    }
}