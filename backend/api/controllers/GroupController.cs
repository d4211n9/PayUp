using api.filters;
using api.models;
using infrastructure.dataModels;
using Microsoft.AspNetCore.Mvc;
using service.services;

namespace api.controllers;

[ApiController]
public class GroupController : ControllerBase
{
    private readonly GroupService _service;

    public GroupController(GroupService service)
    {
        _service = service;
    }
    
    [RequireAuthentication]
    [HttpPost]
    [Route("/api/group/create")]
    public Group CreateGroup([FromBody] Group group)
    {
        var sessionData = HttpContext.GetSessionData();
        return _service.CreateGroup(group, sessionData!);
    }

    [RequireAuthentication]
    [HttpPost]
    [Route("/api/group/invite")]
    public bool InviteUserToGroup([FromBody] GroupInvitation groupInvitation)
    {
        SessionData? sessionData = HttpContext.GetSessionData();
        return _service.InviteUserToGroup(sessionData, groupInvitation);
    }
}