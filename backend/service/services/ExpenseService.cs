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
    
    public Expense CreateExpense(CreateExpenseDto expenseDto)
    {
        if (!_groupRepo.IsUserInGroup(expenseDto.UserId, expenseDto.GroupId)) throw new AuthenticationException();
        return _expenseRepo.CreateExpense(expenseDto);
    }
}