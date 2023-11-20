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

        Group createdGroup;
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Group>(sql,
                new { group.Name, group.Description, group.Image_Url, group.Created_Date });
        }
    }

    public void AddUserToGroup(int userId, int groupId, bool isOwner)
    {
        var sql =
            $@"
            insert into groups.group_members (user_id, group_id, owner) 
            values (@userId, @groupId, @isOwner) 
            returning *;
            ";

        using (var conn = _dataSource.OpenConnection())
        {
            conn.QueryFirst<Group>(sql,
                new { userId, groupId, isOwner });
        }
    }
}