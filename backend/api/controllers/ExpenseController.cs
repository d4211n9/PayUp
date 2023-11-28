using api.filters;
using api.models;
using Microsoft.AspNetCore.Mvc;
using service.services;

namespace api.controllers;

[ApiController]
public class ExpenseController : ControllerBase
{
    //todo get all users from a group so we can send userId on every user on the expense --Done
    //todo Create Expense endpoint (should create an expense in Expense Repo) --Done

    //todo method that takes a list of userId's that are on a expense, ExpenseId and total amount (so it can divide it out on users), and creates UsersOnAmount i db --Done

    //todo GetAllOwedAmountsInGroup should return a list of all users (id, profile url and name) and the total owed amount as a int (should recieve Group id from frontend)
    //todo method that gets all users in a group, 
    //todo find alle usersOnExpense (id og amount) hvor expense id matcher, join med groups så det kun er expenses hvor gruppeId passer.
    //burde give en liste med userId og amount for alle udgifter foretaget i den gruppe.

    //todo method that gets the total amount owed for a user. (takes all usersOnExpense where user_id and group_id matches, Join with Expense table to get the the group id)

    private readonly ExpenseService _service;

    public ExpenseController(ExpenseService service)
    {
        _service = service;
    }

    [RequireAuthentication]
    [HttpPost]
    [Route("/api/expense/")]
    public FullExpense CreateExpense(CreateFullExpense expense)
    {
        return _service.CreateExpense(expense, HttpContext.GetSessionData()!);
    }
    
    [RequireAuthentication]
    [HttpGet]
    [Route("/api/group/{groupId}/expenses")]
    public IEnumerable<FullExpense> GetAllExpenses([FromRoute] int groupId)
    {
        return _service.GetAllExpenses(groupId, HttpContext.GetSessionData()!);
    }

    [RequireAuthentication]
    [HttpGet]
    [Route("/api/group/{groupId}/balances")]
    public IEnumerable<BalanceDto> GetBalances([FromRoute] int groupId)
    {
        return _service.GetBalances(groupId, HttpContext.GetSessionData()!);
    }
}