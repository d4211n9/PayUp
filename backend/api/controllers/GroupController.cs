using api.models;
using Microsoft.AspNetCore.Mvc;
using service.services;

namespace api.controllers;

[ApiController]
public class GroupController(GroupService service) : ControllerBase
{
    [HttpPost]
    [Route("/api/group/create")]
    public Group CreateGroup([FromBody] Group group)
    {
        return service.CreateGroup(group);
    }
}