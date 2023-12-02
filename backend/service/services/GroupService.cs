using System.Data.SqlTypes;
using System.Security.Authentication;
using System.Security;
using api.models;
using infrastructure.dataModels;
using infrastructure.repository;

namespace service.services;

public class GroupService
{
    private readonly GroupRepository _groupRepo;
    private readonly ExpenseRepository _expenseRepo;
    private readonly UserRepository _userRepository;
    private readonly NotificationFacade _notificationFacade;


    public GroupService(GroupRepository groupRepo, ExpenseRepository expenseRepo, UserRepository userRepository, NotificationFacade notificationFacade)
    {
        _groupRepo = groupRepo;
        _expenseRepo = expenseRepo;
        _userRepository = userRepository;
        _notificationFacade = notificationFacade;

    }

    public Group CreateGroup(CreateGroupModel group, SessionData sessionData, string? imageUrl)
    {
        //Create the group
        var createGroup = new CreateGroupModel
        {
            Name = group.Name,
            Description = group.Description,
            CreatedDate = DateTime.UtcNow
        };

        var responseGroup = _groupRepo.CreateGroup(createGroup, imageUrl);
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

    public bool InviteUserToGroup(SessionData? sessionData, GroupInvitation groupInvitation)
    {
        var ownerId = _groupRepo.IsUserGroupOwner(groupInvitation.GroupId);

        if (sessionData.UserId != ownerId)
            throw new SecurityException("You are not allowed to invite users to this group");

        if (_groupRepo.IsUserInGroup(groupInvitation.ReceiverId, groupInvitation.GroupId))
            throw new ArgumentException("User is already in group");
        
        var fullGroupInvitation = new FullGroupInvitation()
        {
            ReceiverId = groupInvitation.ReceiverId,
            GroupId = groupInvitation.GroupId,
            SenderId = ownerId
        };

        var isEmailSent = SendEmailInvite(groupInvitation.GroupId, groupInvitation.ReceiverId);
        if (!isEmailSent)
            throw new SqlNullValueException("send invite per email");
        return _groupRepo.InviteUserToGroup(fullGroupInvitation);
    }

    private bool SendEmailInvite(int groupId, int userId)
    {
        var group = _groupRepo.GetGroupById(groupId);
        var user = _userRepository.GetById(userId);
        return _notificationFacade.SendInviteEmail(group, user.Email);
    }

    public bool AcceptInvite(SessionData sessionData, bool isAccepted, int groupId)
    {
        var user = new UserInGroupDto()
        {
            GroupId = groupId,
            IsOwner = false,
            UserId = sessionData.UserId
        };
        
        if (isAccepted)
        {
            bool isCreated = _groupRepo.AddUserToGroup(user);
            if (!isCreated)
                throw new SqlTypeException();
        }
        return _groupRepo.DeleteInvite(user);
    }
    
    public Group? Update(int groupId, UpdateGroupModel model, string? imageUrl)
    {
        return _groupRepo.Update(groupId, model, imageUrl);
    }

}