using api.models;
using infrastructure.repository;

namespace service.services;

public class GroupService(GroupRepository repository)
{
    public Group CreateGroup(Group group)
    {
        return repository.CreateGroup(group);
    }
}