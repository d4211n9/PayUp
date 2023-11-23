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
                user_id as {nameof(Expense.UserId)}, 
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

    public Expense CreateExpense(CreateExpenseDto expenseDto)
    {
        var sql = $@"
        INSERT INTO expenses.expense (user_id, group_id, description, amount, created_date)
        VALUES (@UserId, @GroupId, @Description, @Amount, @CreatedDate)
        RETURNING
            id AS {nameof(Expense.Id)},
            user_id AS {nameof(Expense.UserId)},
            group_id AS {nameof(Expense.GroupId)},
            description AS {nameof(Expense.Description)},
            amount AS {nameof(Expense.Amount)},
            created_date AS {nameof(Expense.CreatedDate)};
    ";

        try
        {
            using var conn = _dataSource.OpenConnection();
            var expense = conn.QueryFirst<Expense>(sql, new
            {
                UserId = expenseDto.UserId,
                GroupId = expenseDto.GroupId,
                Description = expenseDto.Description,
                Amount = expenseDto.Amount,
                CreatedDate = DateTime.UtcNow
            });
            return expense;
        }
        catch (Exception e)
        {
            throw new SqlTypeException("Could not create expense", e);
        }
    }
}