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
                e.id as {nameof(Expense.Id)}, 
                e.user_id as {nameof(Expense.UserId)}, 
                e.group_id as {nameof(Expense.GroupId)},
                e.description as {nameof(Expense.Description)},
                e.amount as {nameof(Expense.Amount)},
                e.created_date as {nameof(Expense.CreatedDate)},
                u.full_name as {nameof(Expense.FullName)}                
            from expenses.expense as e 
                join users.user as u on e.user_id = u.id 
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
    
    public IEnumerable<UserOnExpense> GetUsersOnExpenses(int groupId)
    {
        var sql =
            $@"
            select 
                uoe.user_id as {nameof(UserOnExpense.UserId)}, 
                expense_id as {nameof(UserOnExpense.ExpenseId)}, 
                uoe.amount as {nameof(UserOnExpense.Amount)}
            from expenses.expense 
                join expenses.user_on_expense as uoe on expense.id = uoe.expense_id 
            where group_id = @groupId;
            ";

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.Query<UserOnExpense>(sql, new { groupId });
        }
        catch (Exception e)
        {
            throw new SqlNullValueException(" read users on expenses from group", e);
        }
    }

    public IEnumerable<BalanceDto> GetBalances(int groupId)
    {
        var sql = 
            $@"
            select 
                uoe.user_id as {nameof(BalanceDto.UserId)}, 
                SUM(uoe.amount) as {nameof(BalanceDto.Amount)}
            from expenses.user_on_expense as uoe
                join expenses.expense as e on uoe.expense_id = e.id
                join groups.group as g on e.group_id = g.id
            where g.id = @groupId
            group by uoe.user_id;
            ";

        try
        {
            using var conn = _dataSource.OpenConnection();
            return conn.Query<BalanceDto>(sql, new { groupId });
        }
        catch (Exception e)
        {
            throw new SqlNullValueException(" read balances from group", e);
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
                CreatedDate = expenseDto.CreatedDate,
            });

            return expense;
        }
        catch (Exception e)
        {
            throw new SqlTypeException("Could not create expense", e);
        }
    }

    public IEnumerable<UserOnExpense?> AddUsersToExpense(IEnumerable<UserOnExpense>? userOnExpense)
    {
        var sql1 = "";
        var expenseId = 0;
        
        if (userOnExpense == null) throw new SqlNullValueException();
        
        var usersOnExpense = userOnExpense.ToList();
        foreach (var uoeDto in usersOnExpense!)
        {
            sql1 += $@"
            INSERT INTO expenses.user_on_expense (user_id, expense_id, amount)
            VALUES ({uoeDto.UserId}, {uoeDto.ExpenseId}, {uoeDto.Amount}); 
            ";
            
            expenseId = uoeDto.ExpenseId;
        }

        var sql2 = $@"
        select 
            user_id as {nameof(UserOnExpense.UserId)}, 
            expense_id as {nameof(UserOnExpense.ExpenseId)},
            amount as {nameof(UserOnExpense.Amount)}
        from expenses.user_on_expense 
        where expense_id = {expenseId} ;
        ";

        try
        {
            using var conn = _dataSource.OpenConnection();
            conn.Execute(sql1);
            return conn.Query<UserOnExpense?>(sql2);
        }
        catch (Exception e)
        {
            throw new SqlTypeException("Could not add users to expense", e);
        }
    }
}