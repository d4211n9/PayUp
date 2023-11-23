using System.Data.SqlTypes;
using System.Security;
using api.models;
using infrastructure.dataModels;
using infrastructure.repository;

namespace service.services;

public class GroupService
{
    private readonly GroupRepository _groupRepo;

    public GroupService(GroupRepository groupRepo)
    {
        _groupRepo = groupRepo;
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

    public bool InviteUserToGroup(SessionData? sessionData, GroupInvitation groupInvitation)
    {
        int ownerId = _groupRepo.IsUserGroupOwner(groupInvitation.GroupId);
        
        if (sessionData.UserId != ownerId)
            throw new SecurityException("You are not allowed to invite users to this group");

        if (_groupRepo.IsUserInGroup(groupInvitation.ReceiverId, groupInvitation.GroupId))
            throw new ArgumentException("User is already in group");

        FullGroupInvitation fullGroupInvitation = new FullGroupInvitation()
        {
            ReceiverId = groupInvitation.ReceiverId,
            GroupId = groupInvitation.GroupId,
            SenderId = ownerId
        };
        
        return _groupRepo.InviteUserToGroup(fullGroupInvitation);
    }
}