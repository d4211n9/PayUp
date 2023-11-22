﻿using api.filters;
using api.TransferModels;
using infrastructure.dataModels;
using Microsoft.AspNetCore.Mvc;
using service.services;

namespace api.controllers;

[ApiController]
public class UsersController : ControllerBase
{
    
    //todo nice to haves:
    //todo get list of usersShort (should be with a max list size, and only reply ith username and picture )


    private readonly UserService _service;

    public UsersController(UserService service)
    {
        _service = service;
    }
    
    [RequireAuthentication]
    [HttpGet]
    [Route("/api/user/currentuser")]
    public User GetLoggedInUser()
    {
        var data = HttpContext.GetSessionData();
        var user = _service.GetLoggedInUser(data!);
        return user;
    }

    [RequireAuthentication]
    [HttpPut]
    [Route("/api/user/profileinfo")]
    public User EditProfileInfo([FromBody] UserInfoDto model)
    {
        var data = HttpContext.GetSessionData();
        var user = _service.EditProfileInfo(data, model);
        return user;
    }
    
    [RequireAuthentication]
    [HttpDelete]
    [Route("/api/account/profile")]
    public ResponseDto DeleteProfile()
    {
        var data = HttpContext.GetSessionData();
        var userDeleted = _service.DeleteAccount(data);
        return new ResponseDto
        {
            ResponseData = userDeleted
        };
    }
}