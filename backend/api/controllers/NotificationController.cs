using System.Runtime.InteropServices.JavaScript;
using api.filters;
using api.models;
using infrastructure.dataModels;
using Microsoft.AspNetCore.Mvc;
using service.services;

namespace api.controllers;

public class NotificationController: ControllerBase
{
    private readonly NotificationService _service;

    public NotificationController(NotificationService service)
    {
        _service = service;
    }
    
    [RequireAuthentication]
    [HttpGet]
    [Route("/api/user/notifications")]
    public IEnumerable<NotificationDto> GetNotifications([FromHeader] DateTime lastUpdated)
    {
        SessionData? sessionData = HttpContext.GetSessionData();

        return _service.GetNotifications(sessionData, lastUpdated);
    }
}