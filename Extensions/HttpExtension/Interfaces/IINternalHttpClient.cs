using ResponsePackage;

namespace Extensions.HttpExtension.Interfaces
{
    public interface IInternalHttpClient
    {
        Task<BaseResponse<HResponse>> SendGet<HResponse>(string apihost, string uri, params (string key, object value)[] props);
        Task<BaseResponse> SendGet(string apihost, string uri, params (string key, object value)[] props);
        Task<BaseResponse<HResponse>> SendPost<HRequest, HResponse>(string apihost, string uri, HRequest requestObject, string contenttype = "application/json");
        Task<BaseResponse> SendPost<HRequest>(string apihost, string uri, HRequest requestObject, string contenttype = "application/json");
    }
}
