using api.models;
using infrastructure.dataModels;
using infrastructure.repository;

namespace service.services;

public class NotificationService
{
    private GroupRepository _groupRepository;
    private NotificationFacade _notificationFacade;
    
    public NotificationService(GroupRepository groupRepository, NotificationFacade notificationFacade)
    {
        _groupRepository = groupRepository;
        _notificationFacade = notificationFacade;
    }

    public IEnumerable<NotificationDto> GetNotifications(SessionData? sessionData, DateTime lastUpdated)
    {
        var notifications = new List<NotificationDto>();
        
        var inviteNotifications = _groupRepository.GetGroupInviteNotifications(sessionData.UserId, lastUpdated);
        foreach (var notification in inviteNotifications)
        {
            var result =NotificationFacade.CreateInviteNotification(notification);
            notifications.Add(result);
        }
        return notifications;
    }
    
}