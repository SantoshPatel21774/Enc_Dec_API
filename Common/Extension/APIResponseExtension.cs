namespace Common.Extension
{
    public class APIResponse
    {
        public bool IsScuccess { get; set; } = false;
        public object? Data { get; set; }
        public string? Message { get; set; }
        public DateTime ResponseTime { get; set; } = DateTime.Now;
    }
    public static class APIResponseExtension
    {
        public static APIResponse ToSuccessResponse(this object data, string msg)
        {
            return new APIResponse
            {
                Data = data,
                IsScuccess= true,
                Message=msg
            };
        }

        public static APIResponse ToFailureResponse(this string msg)
        {
            return new APIResponse
            {
                IsScuccess = false,
                Message = msg,
                Data = null
            };
        }


    }
}
