using System.Text.Json;

namespace ResponsePackage
{
    public class BaseResponse<T>
    {
        public required ResponseCodes Code { get; set; }
        public string CodeMessage { get { return Code.ToString(); } }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
    public class BaseResponse
    {
        public required ResponseCodes Code { get; set; }
        public string CodeMessage { get { return Code.ToString(); } }
        public string? Message { get; set; }
    }
}
