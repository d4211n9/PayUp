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

    public int GetGroupOwnerId(int groupId)
    {
        string sql = @"
               SELECT groups.group_members.user_id
               FROM groups.group_members
               WHERE groups.group_members.group_id = @groupId
               AND groups.group_members.owner = true;";

        try
        {
            NpgsqlConnection conn = _dataSource.OpenConnection();
            return conn.QueryFirstOrDefault<int>(sql, new { groupId });
        }
        catch (Exception e)
        {
            throw new SqlTypeException("Failed to retrieve owner ID of the group");
        }
    }
}