using System.Data.SqlTypes;
using System.Security.Authentication;
using api.models;
using infrastructure.dataModels;
using infrastructure.repository;

namespace service.services;

public class GroupService
{
    private readonly GroupRepository _groupRepo;
    private readonly ExpenseRepository _expenseRepo;

    public GroupService(GroupRepository groupRepo, ExpenseRepository expenseRepo)
    {
        _groupRepo = groupRepo;
        _expenseRepo = expenseRepo;
    }

    public Group CreateGroup(Group group, SessionData sessionData)
    {
        //Create the group
        group.Created_Date = DateTime.UtcNow;
        var responseGroup = _groupRepo.CreateGroup(group);
        if (ReferenceEquals(responseGroup, null)) throw new SqlNullValueException(" create group");

        //Add the creator as member(owner) in the group
        var addedToGroup = _groupRepo.AddUserToGroup(sessionData.UserId, responseGroup.Id, true);
        if (!addedToGroup) throw new SqlNullValueException(" add user to the group");
        return responseGroup;
    }

    public IEnumerable<Expense> GetAllExpenses(int groupId, SessionData sessionData)
    { 
        if (!_groupRepo.IsUserInGroup(sessionData.UserId, groupId)) throw new AuthenticationException();
        return _expenseRepo.GetAllExpenses(groupId);
    }
}