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

    public IEnumerable<Expense> GetAllExpenses(int groupId)
    {
        var sql =
            $@"
            select 
                id as {nameof(Expense.Id)}, 
                group_id as {nameof(Expense.GroupId)},
                description as {nameof(Expense.Description)},
                amount as {nameof(Expense.Amount)},
                created_date as {nameof(Expense.CreatedDate)}
            from expenses.expense 
            where group_id = @groupId;
            ";

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.Query<Expense>(sql, new { groupId });
        }
        catch (Exception e)
        {
            throw new SqlNullValueException(" read expenses from group", e);
        }
    }
}