﻿using api.filters;
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
    [HttpGet]
    [Route("/api/group/{groupId}")]
    public Group GetGroupById([FromRoute] int groupId)
    {
        return _service.GetGroupById(groupId, HttpContext.GetSessionData()!);
    }

    [RequireAuthentication]
    [HttpGet]
    [Route("/api/mygroups")]
    public IEnumerable<Group> GetMyGroups()
    {
        var sessionData = HttpContext.GetSessionData();
        var userId = sessionData.UserId;
        return _service.GetMyGroups(userId);
    }

    [RequireAuthentication]
    [HttpPost]
    [Route("/api/group/invite")]
    public bool InviteUserToGroup([FromBody] GroupInvitation groupInvitation)
    {
        bool success = _service.InviteUserToGroup(HttpContext.GetSessionData(), groupInvitation);
        if (success) HttpContext.Response.StatusCode = StatusCodes.Status201Created;
        return success;
    }
    
    [RequireAuthentication]
    [HttpGet]
    [Route("/api/group/{groupId}/users")]
    public IEnumerable<ShortUserDto> GetUsersInGroup([FromRoute] int groupId)
    {
        return _service.GetUsersInGroup(groupId, HttpContext.GetSessionData()!);
    }
}