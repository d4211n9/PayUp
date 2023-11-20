using api.filters;
using api.models;
using Microsoft.AspNetCore.Mvc;
using service.services;

namespace api.controllers;

[ApiController]
public class GroupController(GroupService service) : ControllerBase
{
    [RequireAuthentication]
    [HttpPost]
    [Route("/api/group/create")]
    public Group CreateGroup([FromBody] Group group)
    {
        var sessionData = HttpContext.GetSessionData();
        return service.CreateGroup(group, sessionData!);
    }
    
    
}