using System.Collections;
using System.Security.Authentication;
using api.models;
using infrastructure.dataModels;
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

    public FullExpense CreateExpense(CreateFullExpense createFullExpense, SessionData sessionData)
    {
        if (!_groupRepo.IsUserInGroup(createFullExpense.Expense.UserId, createFullExpense.Expense.GroupId))
            throw new AuthenticationException();
        var responseExpense = _expenseRepo.CreateExpense(createFullExpense.Expense);

        // Fordeling af expense amount ud på antal brugere
        var numberOfUsers = createFullExpense.UserIdsOnExpense.Count();
        var share = createFullExpense.Expense.Amount / numberOfUsers;

        var userList = new List<CreateUserOnExpense>();
        foreach (var u in createFullExpense.UserIdsOnExpense)
        {
            if (createFullExpense.Expense.UserId == u)
            {
                var user = new CreateUserOnExpense()
                {
                    Amount = (share * numberOfUsers) - share,
                    ExpenseId = responseExpense.Id,
                    UserId = u
                };
                userList.Add(user);
            }
            else
            {
                var user = new CreateUserOnExpense()
                {
                    Amount = share * -1,
                    ExpenseId = responseExpense.Id,
                    UserId = u
                };
                userList.Add(user);
            }
        }

        var usersOnExpense = _expenseRepo.AddUsersToExpense(userList);

        var fullExpense = new FullExpense()
        {
            Expense = responseExpense,
            UsersOnExpense = usersOnExpense!,
            LoggedInUser = sessionData.UserId
        };
        return fullExpense;
    }

    public IEnumerable<FullExpense> GetAllExpenses(int groupId, SessionData sessionData)
    {
        //Assert logged in user is authorized to access this group (api checked authentication)
        if (!_groupRepo.IsUserInGroup(sessionData.UserId, groupId)) throw new AuthenticationException();

        //Query all expenses & users on expenses from db
        IEnumerable<Expense> expenseDtos = _expenseRepo.GetAllExpenses(groupId).ToList();
        IEnumerable<GetUserOnExpense> usersOnExpenses = _expenseRepo.GetUsersOnExpenses(groupId).ToList();

        var fullExpenses = new List<FullExpense>();
        var userId = sessionData.UserId;

        //Loop through each expense
        foreach (var expense in expenseDtos)
        {
            //Create a temp list of users linked to the current expense from outer loop
            List<GetUserOnExpense> usersOnExpense = new List<GetUserOnExpense>();

            //Loop through each user on expense entry
            foreach (var uoe in usersOnExpenses)
            {
                //Add linked users to the temp list
                if (expense.Id.Equals(uoe.ExpenseId))
                {
                    usersOnExpense.Add(uoe);
                }
            }

            //Combine the expense with the list of linked users to make a full expense
            FullExpense fullExpense = new FullExpense()
            {
                Expense = expense,
                UsersOnExpense = usersOnExpense!,
                LoggedInUser = userId
            };

            //Add to the final list of full expenses
            fullExpenses.Add(fullExpense);
        }

        return fullExpenses;
    }

    public IEnumerable<BalanceDto> GetBalances(int groupId, SessionData sessionData)
    {
        //Assert logged in user is authorized to access this group (api checked authentication)
        if (!_groupRepo.IsUserInGroup(sessionData.UserId, groupId)) throw new AuthenticationException();

        var balances = _expenseRepo.GetBalances(groupId);
        
        var payers = new Dictionary<int, decimal>();
        var payees = new Dictionary<int, decimal>();
        
        foreach (var b in balances)
        {
            if (b.Amount < 0) {
                payers.Add(b.UserId, b.Amount);
            } else if (b.Amount > 0) {
                payees.Add(b.UserId, b.Amount);
            } 
        }

        while (payees.Count < 0 && payers.Count < 0)
        {
            //Først find min i payer, og max i payee
            //Så skal de modregnes og der skal laves et transaction dto
            //Repeat loop

            var lowest = payers.Values.Min();
            foreach (var payer in payers)
            {
                
            }
        }

        //TODO Husk at tjek både payee og payer er i nul
        return balances;
    }
}