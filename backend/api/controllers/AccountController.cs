using api.models;
using api.TransferModels;
using infrastructure.dataModels;
using Microsoft.AspNetCore.Mvc;
using service.services;

namespace api.controllers;

[ApiController]
public class AccountController(AccountService service, JwtService jwtService) : ControllerBase
{
    [HttpPost]
    [Route("/api/account/register")]
    public ResponseDto Register([FromBody] RegisterModel model)
    {
        var user = service.Register(model);
        return new ResponseDto
        {
            MessageToClient = "Successfully registered",
        };
    }

    [HttpPost]
    [Route("/api/account/login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        var user = service.Authenticate(model);
        if (user == null) return Unauthorized();

        var token = jwtService.IssueToken(SessionData.FromUser(user!));
        return Ok(new { token });
    }

    [HttpPost] //todo should take the email from body and send, and send an 200 status code if password is resend.
    [Route("/api/account/recover")]
    public IActionResult RecoverAccount([FromBody] LoginModel model)
    {
        throw new NotImplementedException("not implemented yet");
    }
}