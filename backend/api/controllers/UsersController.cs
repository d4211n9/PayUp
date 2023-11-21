using api.filters;
using api.TransferModels;
using infrastructure.dataModels;
using Microsoft.AspNetCore.Mvc;
using service.services;

namespace api.controllers;

public class UsersController : ControllerBase
{
    //todo need these for profile view
    //todo edit picture (because it can be heavy to upload a picture)
    
    //todo nice to haves:
    //todo get list of usersShort (should be with a max list size, and only reply ith username and picture )
    //todo get user full detail (should check if you are in a group together before returning info)


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
    [Route("/api/account/profileinfo")]
    public ResponseDto EditProfileInfo([FromBody] UserInfoDto model)
    {
        var data = HttpContext.GetSessionData();
        var user = _service.EditProfileInfo(data, model);
        return new ResponseDto
        {
            ResponseData = user
        };
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