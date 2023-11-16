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
        //TODO When userId is changed to int remove hard-coding
        var data = HttpContext.GetSessionData();
        return service.CreateGroup(group, 1);
    }
}