using api.models;
using Dapper;
using Npgsql;

namespace infrastructure.repository;

public class GroupRepository(NpgsqlDataSource dataSource)
{
    public Group CreateGroup(Group group)
    {
        var sql =
            $@"
            insert into groups.group (name, description, image_url, created_date) 
            values (@name, @description, @imageUrl, @createdDate) 
            returning *;
            ";

        Group createdGroup;
        using (var conn = dataSource.OpenConnection())
        {
            return conn.QueryFirst<Group>(sql,
                new { group.Name, group.Description, group.ImageUrl, group.CreatedDate });
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

        using (var conn = dataSource.OpenConnection())
        {
            conn.QueryFirst<Group>(sql,
                new { userId, groupId, isOwner });
        }
    }
}