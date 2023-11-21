using System.Data.SqlTypes;
using api.models;
using Dapper;
using Npgsql;

namespace infrastructure.repository;

public class ExpenseRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public ExpenseRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<Expense> GetAllExpenses(int group_Id)
    {
        var sql =
            $@"
            select * from expenses.expense 
            where group_id = @group_Id;
            ";

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.Query<Expense>(sql, new { group_Id });
        }
        catch (Exception e)
        {
            throw new SqlNullValueException(" read expenses from group", e);
        }
    }
}