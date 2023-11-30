using api.models;
using infrastructure.dataModels;
using infrastructure.repository;

namespace service.services;

public class NotificationService
{
    private GroupRepository _groupRepository;
    
    public NotificationService(GroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public IEnumerable<NotificationDto> GetNotifications(SessionData? sessionData, DateTime lastUpdated)
    {
        var notifications = new List<NotificationDto>();
        
        var inviteNotifications = _groupRepository.GetGroupInviteNotifications(sessionData.UserId, lastUpdated);
        foreach (var notification in inviteNotifications)
        {
            var result = NotificationFacade.CreateInviteNotification(notification);
            notifications.Add(result);
        }
        return notifications;
    }
    
}