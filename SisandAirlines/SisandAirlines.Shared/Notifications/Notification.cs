using System.Net;

namespace SisandAirlines.Shared.Notifications
{
    public class Notification
    {
        public Notification(string message, HttpStatusCode statusCode)
        {
            Message = message;
            StatusCode = (int)statusCode;
        }

        public string? Message { get; }
        public int StatusCode { get; }
    }
}
