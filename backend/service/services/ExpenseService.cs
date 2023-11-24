using System.Security.Authentication;
using api.models;
using infrastructure.repository;

namespace service.services;

public class ExpenseService
{
    private readonly GroupRepository _groupRepo;
    private readonly ExpenseRepository _expenseRepo;
    private readonly UserRepository _userRepository;

    public ExpenseService(GroupRepository groupRepo, ExpenseRepository expenseRepo, UserRepository userRepo)
    {
        _groupRepo = groupRepo;
        _expenseRepo = expenseRepo;
        _userRepository = userRepo;
    }

    public FullExpense CreateExpense(CreateFullExpense createFullExpense)
    {
        if (!_groupRepo.IsUserInGroup(createFullExpense.Expense.UserId, createFullExpense.Expense.GroupId))
            throw new AuthenticationException();
        Expense responseExpense = _expenseRepo.CreateExpense(createFullExpense.Expense);

        IEnumerable<UserOnExpenseDto?> userOnExpenseDtos =
            _expenseRepo.AddUsersToExpense(createFullExpense.UsersOnExpense);

        FullExpense fullExpense = new FullExpense() { Expense = responseExpense, UsersOnExpense = userOnExpenseDtos! };
        return fullExpense;
    }
}