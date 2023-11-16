using api.models;
using infrastructure.repository;
using Microsoft.Extensions.Logging;

namespace service.services;

public class GroupService(
    GroupRepository repository,
    ILogger<GroupService> logger)
{
    public Group CreateGroup(Group group, int userId)
    {
        try
        {
            Group createdGroup = repository.CreateGroup(group);
            repository.AddUserToGroup(userId, createdGroup.Id, true);
            return createdGroup;
        }
        catch (Exception e)
        {
            logger.LogError("Create error: {Message}", e);
            throw new Exception("Could not create group");
        }
        
    }
}