using SisandAirlines.Shared.Models;

namespace SisandAirlines.Shared.Factories
{
    public static class ResponseFactory
    {
        public static ResponseData Success(string message) =>
            new ResponseData(ResponseData.DefaultSucessStatus, message);

        public static ResponseData Error(string message) =>
            new ResponseData(ResponseData.DefaultErrorStatus, message);

        public static ResponseData NotFound(string message) =>
            new ResponseData(ResponseData.DefaultErrorStatus, message); 

        public static ResponseData<T> Success<T>(T data, string message = "") =>
            new ResponseData<T>(data, ResponseData.DefaultSucessStatus, message);

        public static ResponseData<T> Error<T>(string message, T? data = default) =>
            new ResponseData<T>(data, ResponseData.DefaultErrorStatus, message);
    }
}
