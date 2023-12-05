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
        var loggedInUser = sessionData.UserId;
        if (!_groupRepo.IsUserInGroup(loggedInUser, createFullExpense.Expense.GroupId))
            throw new AuthenticationException();
        
        if (!createFullExpense.UserIdsOnExpense.Contains(loggedInUser)) throw new AuthenticationException();
        
        createFullExpense.Expense.UserId = loggedInUser;
        
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
        return _expenseRepo.GetBalances(groupId);
    }

    public IEnumerable<Transaction> GetTotalTransactions(int groupId, SessionData sessionData)
    {
        //Assert logged in user is authorized to access this group (api checked authentication)
        if (!_groupRepo.IsUserInGroup(sessionData.UserId, groupId)) throw new AuthenticationException();

        var balances = _expenseRepo.GetBalances(groupId);
                
        var payers = new Dictionary<int, decimal>();
        var payees = new Dictionary<int, decimal>();
        var transactionList = new List<Transaction>(); 

        //filters users by amount into payers and payees
        foreach (var b in balances)
        {
            if (b.Amount < 0) {
                payers.Add(b.UserId, b.Amount);
            } else if (b.Amount > 0) {
                payees.Add(b.UserId, b.Amount);
            } 
        }
        
        //loops through the payers and payees until all users are square
        while (payers.Count > 0 && payees.Count > 0)
        {
            var lowestPayerKey = payers.OrderBy(pair => pair.Value).FirstOrDefault().Key;
            var highestPayeeKey = payees.OrderByDescending(pair => pair.Value).FirstOrDefault().Key;
            
            if (lowestPayerKey == 0 || highestPayeeKey == 0)
            {
                // Handle cases where a key is not found (e.g., dictionaries are empty).
                break;
            }
            //gets the amounts for both payer and payee
            decimal payerAmount = payers[lowestPayerKey];
            decimal payeeAmount = payees[highestPayeeKey];
            //gets the amount that has to payed before one part is square
            decimal settledAmount = Math.Min(Math.Abs(payerAmount), payeeAmount);

            // Perform the settlement
            // Update payer's amount
            payers[lowestPayerKey] += settledAmount;

            // Update payee's amount
            payees[highestPayeeKey] -= settledAmount;
            
            string payerName = GetUserNameFromBalances(lowestPayerKey, balances);
            string payeeName = GetUserNameFromBalances(highestPayeeKey, balances);

            //creates a transAction Object, to keep track of transactions
            var transAction = new Transaction
            {
                PayeeId = highestPayeeKey,
                PayerId = lowestPayerKey,
                Amount = settledAmount,
                PayerName = payerName,
                PayeeName = payeeName
            };
            transactionList.Add(transAction);

            // Remove payer or payee if the amount becomes zero
            if (payers[lowestPayerKey] == 0)
            {
                payers.Remove(lowestPayerKey);
            }

            if (payees[highestPayeeKey] == 0)
            {
                payees.Remove(highestPayeeKey);
            }
        }
        return transactionList;
    }
    
    private string GetUserNameFromBalances(int userId, IEnumerable<BalanceDto> balances)
    {
        var userBalance = balances.FirstOrDefault(b => b.UserId == userId);
        // Return the user name if found, otherwise a default value or an empty string
        return userBalance != null ? userBalance.FullName : string.Empty;
    }
}