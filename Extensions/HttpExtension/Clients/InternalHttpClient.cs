using Extensions.HttpExtension.Interfaces;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ResponsePackage;
using System.Net.Http.Json;
using System.Text;

namespace Extensions.HttpExtension.Clients
{
    public class InternalHttpClient(HttpClient httpClient, ILogger<InternalHttpClient> logger) : IInternalHttpClient
    {
        private readonly HttpClient httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        private readonly ILogger<InternalHttpClient> logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Method to send HTTP GET request to internal API service with response type
        /// </summary>
        /// <typeparam name="HResponse">Response type object</typeparam>
        /// <param name="apihost">API host to request</param>
        /// <param name="uri">URI to controller</param>
        /// <param name="props">URI parameters</param>
        /// <returns>Request of HResponse type</returns>
        public async Task<BaseResponse<HResponse>> SendGet<HResponse>(string apihost, string uri, params (string key, object value)[] props)
        {
            logger.LogInformation("[Internal] Sending HTTP GET request to host {apihost} with route {uri} and return type {type} with props:", apihost, uri, typeof(HResponse).Name);
            string httpstring = $"http://{apihost}:8080/internal/{uri}" + (props.Length > 0 ? "?" : "");
            foreach (var (key, value) in props)
            {
                logger.LogInformation("{key} = {value}", key, value);
                httpstring += key + "=" + value + "&";
            }
            httpstring = httpstring.Trim('&');

            string response = await httpClient.GetStringAsync(httpstring);
            if (response is null)
            {
                logger.LogError("[Internal] Exception occured while trying to get response from {apihost} with uri {uri}", apihost, uri);
                throw new CustomExceptionResponse(ResponseCodes.UndefinedServerException, $"{apihost} returned null response"); 
            }
            var obj = JsonConvert.DeserializeObject<BaseResponse<HResponse>>(response);
            if (obj is null)
            {
                logger.LogError("[Internal] Can't serialize http response of {apihost} with uri {uri}", apihost, uri);
                throw new CustomExceptionResponse(ResponseCodes.UndefinedServerException, $"can't deserialize response from {apihost}");
            }
            logger.LogInformation("[Internal] HTTP GET request to {apihost} with route {uri} completed successfully", apihost, uri);
            return obj;
        }
        /// <summary>
        /// Method to send HTTP GET request to internal API service without response type
        /// </summary>
        /// <typeparam name="HResponse">Response type object</typeparam>
        /// <param name="apihost">API host to request</param>
        /// <param name="uri">URI to controller</param>
        /// <param name="props">URI parameters</param>
        /// <returns>Request of HResponse type</returns>
        public async Task<BaseResponse> SendGet(string apihost, string uri, params (string key, object value)[] props)
        {
            logger.LogInformation("[Internal] Sending HTTP GET request to host {apihost} with route {uri} with default response...", apihost, uri);

            string httpstring = $"http://{apihost}:8080/internal/{uri}" + (props.Length > 0 ? "?" : "");
            foreach (var (key, value) in props)
            {
                logger.LogInformation("{key} = {value}", key, value);
                httpstring += key + "=" + value + "&";
            }
            httpstring = httpstring.Trim('&'); 

            string response = await httpClient.GetStringAsync(httpstring);
            if (response is null)
            {
                logger.LogError("[Internal] Exception occured while trying to get response from {apihost} with uri {uri}", apihost, uri);
                throw new CustomExceptionResponse(ResponseCodes.UndefinedServerException, $"{apihost} returned null response");
            }
            var obj = JsonConvert.DeserializeObject<BaseResponse>(response);
            if (obj is null)
            {
                logger.LogError("[Internal] Can't serialize http response of {apihost} with uri {uri}", apihost, uri);
                throw new CustomExceptionResponse(ResponseCodes.UndefinedServerException, $"can't deserialize response from {apihost}"); 
            }
            logger.LogInformation("[Internal] HTTP GET request to {apihost} with route {uri} completed successfully", apihost, uri);
            return obj;
        }
        public async Task<BaseResponse<HResponse>> SendPost<HRequest, HResponse>(string apihost, string uri, HRequest requestObject, string contenttype = "application/json")
        {
            logger.LogInformation("[Internal] Sending HTTP POST request to host {apihost} with route {uri} with {name} response...", apihost, uri, typeof(HResponse).Name);
            HttpContent content = new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, contenttype);
            HttpResponseMessage response = await httpClient.PostAsync($"http://{apihost}:8080/internal/{uri}", content);
            if(response is null)
            {
                logger.LogError("[Internal] Exception occured while trying to get response from {apihost} with uri {uri}", apihost, uri);
                throw new CustomExceptionResponse(ResponseCodes.UndefinedServerException, $"{apihost} returned null response");
            }
            var obj = JsonConvert.DeserializeObject<BaseResponse<HResponse>>(await response.Content.ReadAsStringAsync());
            if (obj is null)
            {
                logger.LogError("[Internal] Can't serialize http response of {apihost} with uri {uri}", apihost, uri);
                throw new CustomExceptionResponse(ResponseCodes.UndefinedServerException, $"can't deserialize response from {apihost}");
            }
            logger.LogInformation("[Internal] HTTP GET request to {apihost} with route {uri} completed successfully", apihost, uri);
            return obj;
        }
        public async Task<BaseResponse> SendPost<HRequest>(string apihost, string uri, HRequest requestObject, string contenttype = "application/json")
        {
            logger.LogInformation("[Internal] Sending HTTP POST request to host {apihost} with route {uri} with default response...", apihost, uri);
            HttpContent content = new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, contenttype);
            HttpResponseMessage response = await httpClient.PostAsync($"http://{apihost}:8080/internal/{uri}", content);
            if (response is null)
            {
                logger.LogError("[Internal] Exception occured while trying to get response from {apihost} with uri {uri}", apihost, uri);
                throw new CustomExceptionResponse(ResponseCodes.UndefinedServerException, $"{apihost} returned null response");
            }
            var obj = JsonConvert.DeserializeObject<BaseResponse>(await response.Content.ReadAsStringAsync());
            if (obj is null)
            {
                logger.LogError("[Internal] Can't serialize http response of {apihost} with uri {uri}", apihost, uri);
                throw new CustomExceptionResponse(ResponseCodes.UndefinedServerException, $"can't deserialize response from {apihost}");
            }
            logger.LogInformation("[Internal] HTTP GET request to {apihost} with route {uri} completed successfully", apihost, uri);
            return obj;
        }
    }
}