using api.models;
using infrastructure.dataModels;
using infrastructure.repository;
using Microsoft.Extensions.Logging;

namespace service.services;

public class GroupService
{
    private readonly GroupRepository _groupRepo; //todo should be like this so we can make them readonly and private..
    private readonly UserService _userService;   //todo also makes it easy to see that is i an instance variable
    public GroupService(GroupRepository groupRepo, UserService userService)
    {
        _groupRepo = groupRepo;
        _userService = userService;
    }
    
    public Group CreateGroup(Group group, SessionData sessionData)
    { //todo missing error handling (wait for jenny)
        Group createdGroup;
        
        try //Create the group
        {
            group.Created_Date = DateTime.UtcNow;
            createdGroup = _groupRepo.CreateGroup(group);
        }
        catch (Exception e)
        {
            throw new Exception("Could not create group");
        }

        try //Add the creator as member(owner) in the group
        {
            //todo the repo method should not return everything when no values are used,
            //todo should instead return a bool, that is being checked, before the group is returned to api layer
            _groupRepo.AddUserToGroup(sessionData.UserId, createdGroup.Id, true);
        }
        catch (Exception e)
        {
            throw new Exception("Could not add user to the group");
        }
        
        return createdGroup;
    }

    public IEnumerable<Group> GetMyGroups(int userId)
    {
        return _groupRepo.GetMyGroups(userId);
    }
}