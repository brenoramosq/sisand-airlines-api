using SisandAirlines.Shared.Notifications;

namespace SisandAirlines.Shared.Interfaces
{
    public interface INotificator
    {
        void Add(Notification notification);
        List<Notification> GetNotifications();
        bool HasNotification();
    }
}
