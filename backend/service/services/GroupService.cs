using api.models;
using infrastructure.dataModels;
using infrastructure.repository;
using Microsoft.Extensions.Logging;

namespace service.services;

public class GroupService(
    GroupRepository repository,
    ILogger<GroupService> logger,
    AccountService accountService)
{
    public Group CreateGroup(Group group, SessionData sessionData)
    {
        Group createdGroup;
        
        try //Create the group
        {
            group.Created_Date = DateTime.UtcNow;
            createdGroup = repository.CreateGroup(group);
        }
        catch (Exception e)
        {
            throw new Exception("Could not create group");
        }

        try //Add the creator as member(owner) in the group
        {
            var user = accountService.Get(sessionData);
            repository.AddUserToGroup(user!.Id, createdGroup.Id, true);
        }
        catch (Exception e)
        {
            throw new Exception("Could not add user to the group");
        }
        
        return createdGroup;
    }
}