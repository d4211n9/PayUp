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
    //todo metode til at chekke for ny invite noti hvis time er null skal alle sendes
    //skal kunne lave alle noti om til et dto object inden det bliver sendt --kald facade 
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