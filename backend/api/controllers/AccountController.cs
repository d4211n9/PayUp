using api.models;
using api.TransferModels;
using Microsoft.AspNetCore.Mvc;
using service.services;

namespace api.controllers;

[ApiController]
public class AccountController: ControllerBase
{
    
    private readonly AccountService _service;

    
    public AccountController(AccountService service)
    {
        _service = service;
    }
    
    [HttpPost]
    [Route("/api/account/register")]
    public ResponseDto Register([FromBody] RegisterModel model)
    {
        var user = _service.Register(model.Email, model.FullName, model.Password, model.PhoneNumber, model.ProfileUrl);
        return new ResponseDto
        {
            MessageToClient = "Successfully registered"
        };
    }
    
    
    
    
    [HttpPost]//todo should create token and send token to frontend 
    [Route("/api/account/login")]
    public ResponseDto Login([FromBody] LoginModel model)
    {
        var user = _service.Authenticate(model.Email, model.Password);
        return new ResponseDto
        {
            MessageToClient = "Successfully authenticated"
        };
    }
    
    [HttpPost]//todo should take the email from body and send, and send an 200 status code if password is resend.
    [Route("/api/account/recover")]
    public IActionResult RecoverAccount([FromBody] LoginModel model)
    {
        throw new NotImplementedException("not implemented yet");
    }
    

}