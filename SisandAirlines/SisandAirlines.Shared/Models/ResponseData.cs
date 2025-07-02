namespace SisandAirlines.Shared.Models
{
    public class ResponseData
    {
        public const string DefaultSucessStatus = "Success";
        public const string DefaultErrorStatus = "Error";
   

        public ResponseData(string status, string message)
        {
            Status = status;
            Message = message;
        }

        public string Status { get; set; }
        public string Message { get; set; }
    }

    public class ResponseData<T> : ResponseData
    {
        public ResponseData(T data) : this(data, DefaultErrorStatus, string.Empty)
        {
                
        }

        public ResponseData(T data, string status, string message) : base(status, message)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
