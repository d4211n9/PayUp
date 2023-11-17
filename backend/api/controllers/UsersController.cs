using api.filters;
using api.TransferModels;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers;

public class UsersController
{
    //todo need these for profile view
    //todo edit user (only username email and phone)
    //todo edit picture (because it can be heavy to upload a picture)
    //todo get logged in user 
    //todo delete log in profile 
    
    [RequireAuthentication]
    [HttpGet]
    [Route("/api/account/getloggedinuser")]
    public ResponseDto GetLoggedInUser()
    {
        throw new NotImplementedException();
        /*
        var data = HttpContext.GetSessionData();
        var user = _service.Get(data!);
        return new ResponseDto
        {
            ResponseData = user
        };
        */
    }
    
    
    //todo nice to haves:
    //todo get list of usersShort (should be with a max list size, and only reply ith username and picture )
    //todo get user full detail (should check if you are in a group together before returning info)
    
    
}