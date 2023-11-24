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
    private readonly UserRepository _userRepository;

    public GroupService(GroupRepository groupRepo, ExpenseRepository expenseRepo, UserRepository userRepository)
    {
        _groupRepo = groupRepo;
        _expenseRepo = expenseRepo;
        _userRepository = userRepository;
    }

    public Group CreateGroup(Group group, SessionData sessionData)
    {
        //Create the group
        group.CreatedDate = DateTime.UtcNow;
        var responseGroup = _groupRepo.CreateGroup(group);
        if (ReferenceEquals(responseGroup, null)) throw new SqlNullValueException(" create group");

        //Add the creator as member(owner) in the group
        UserInGroupDto userInGroupDto = new UserInGroupDto()
            { UserId = sessionData.UserId, GroupId = responseGroup.Id, IsOwner = true };
        var addedToGroup = _groupRepo.AddUserToGroup(userInGroupDto);
        if (!addedToGroup) throw new SqlNullValueException(" add user to the group");
        return responseGroup;
    }

    public Group GetGroupById(int groupId, SessionData sessionData)
    {
        if (!_groupRepo.IsUserInGroup(sessionData.UserId, groupId)) throw new AuthenticationException();
        return _groupRepo.GetGroupById(groupId);
    }

    public IEnumerable<Group> GetMyGroups(int userId)
    {
        return _groupRepo.GetMyGroups(userId);
    }

    public IEnumerable<ShortUserDto> GetUsersInGroup(int groupId, SessionData sessionData)
    {
        if (!_groupRepo.IsUserInGroup(sessionData.UserId, groupId)) throw new AuthenticationException();
        return _userRepository.GetAllMembersOfGroup(groupId);
    }
}