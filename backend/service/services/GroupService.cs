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
        try
        {
            var user = accountService.Get(sessionData);
            Group createdGroup = repository.CreateGroup(group);
            repository.AddUserToGroup(user!.Id, createdGroup.Id, true);
            return createdGroup;
        }
        catch (Exception e)
        {
            logger.LogError("Create error: {Message}", e);
            throw new Exception("Could not create group");
        }
        
    }
}