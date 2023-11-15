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
            insert into groups.group (Name, Description, ImageUrl, CreatedDate) 
            values (@name, @description, @imageUrl, @createdDate) 
            returning *;
            ";

        using (var conn = dataSource.OpenConnection())
        {
            return conn.QueryFirst<Group>(sql,
                new { group.Name, group.Description, group.ImageUrl, group.CreatedDate });
        }
    }
}