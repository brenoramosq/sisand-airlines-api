using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SisandAirlines.Shared.Interfaces;
using SisandAirlines.Shared.Notifications;
using System.Net;
using System.Reflection;

namespace SisandAirlines.Api.Controllers
{
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly INotificator _notificator;

        public MainController(INotificator notificator)
        {
            _notificator = notificator;
        }

        protected bool ValidOperation() => !_notificator.HasNotification();

        protected ActionResult CustomResponse(HttpStatusCode statusCode = HttpStatusCode.OK, object result = null)
        {
            if (ValidOperation())
            {
                return new ObjectResult(result)
                {
                    StatusCode = Convert.ToInt32(statusCode),
                };
            }

            string methodName = Enum.GetName(typeof(HttpStatusCode), _notificator.GetNotifications().Select(n => n.StatusCode).FirstOrDefault());

            MethodInfo? method = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                               .FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == 1);

            if (methodName is not null)
                return (ActionResult)method.Invoke(this, new object[] { new { errors = _notificator.GetNotifications().Select(n => n.Message) } });

            return StatusCode((int)HttpStatusCode.InternalServerError, new
            {
                errors = _notificator.GetNotifications().Select(n => n.Message)
            });
        }

        protected void NotifyError(string message, HttpStatusCode statusCode) => _notificator.Add(new Notification(message, statusCode));
    }
}
