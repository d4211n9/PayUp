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
        //todo missing error handling (wait for jenny)
        //Create the group
        group.Created_Date = DateTime.UtcNow;
        var responseGroup = _groupRepo.CreateGroup(group);
        if (ReferenceEquals(responseGroup, null)) throw new Exception("Could not create group");

        //Add the creator as member(owner) in the group
        var addedToGroup = _groupRepo.AddUserToGroup(sessionData.UserId, responseGroup.Id, true);
        if (!addedToGroup) throw new Exception("Could not add user to the group");

        return responseGroup;
    }
}