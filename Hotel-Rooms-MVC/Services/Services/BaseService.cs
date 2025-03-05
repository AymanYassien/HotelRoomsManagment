using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using hotel_room_api;

using Hotel_Rooms_MVC.Services.IServices;
using Newtonsoft.Json;
using RoomsUtility;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace Hotel_Rooms_MVC.Services.Services;

public class BaseService : IBaseService
{
    public APIResponse responseModel { get; set; }
    public IHttpClientFactory httpClient { get; set; }

    public BaseService(IHttpClientFactory httpClient)
    {
        this.responseModel = new APIResponse();
        this.httpClient = httpClient;
    }
    
    public async Task<T> SendAsync<T>(ApiRequest apiRequest)
    {
        try
        {
            var client = httpClient.CreateClient("RoomApi");
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(apiRequest.Url);
            if (apiRequest.data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.data),
                    Encoding.UTF8, "application/json");
            }

            switch (apiRequest.ApiType)
            {
                case StaticData.ApiTypes.GET:
                    message.Method = HttpMethod.Get;
                    break;
                case StaticData.ApiTypes.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case StaticData.ApiTypes.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                case StaticData.ApiTypes.PUT:
                    message.Method = HttpMethod.Put;
                    break;
            }

            if (!string.IsNullOrEmpty(apiRequest.token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", apiRequest.token);
            }

            HttpResponseMessage apiResponse = null;
            
            apiResponse = await client.SendAsync(message);
            
            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            try
            {
                APIResponse ApiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                if (apiResponse?.StatusCode == HttpStatusCode.NotFound ||
                    apiResponse?.StatusCode == HttpStatusCode.BadRequest)
                {
                    ApiResponse.StatusCode = HttpStatusCode.BadRequest;
                    ApiResponse.IsSuccess = false;
                    var res = JsonConvert.SerializeObject(ApiResponse);
                    var returnObj = JsonConvert.DeserializeObject<T>(res);
                    return returnObj;
                    
                }
            }
            catch (Exception e)
            {
                var E_ApiResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return E_ApiResponse;
            }
            
            var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
            return APIResponse;
        }
        catch (Exception e)
        {
            var dto = new APIResponse()
            {
                ErrorMessages = new List<string>() { Convert.ToString(e.Message) },
                IsSuccess = false
            };

            var res = JsonConvert.SerializeObject(dto);
            var apiResponse = JsonConvert.DeserializeObject<T>(res);
            return apiResponse;
        }
    }
}

// How Consume
/*
 * add Models & DTO
 * add url for api into json settings
 * add API Request class (method, uri, data)
 * add Service + implement it into class service
 *
 * SendAsync implementation:
 * into try catch:
 * 
 * 
 */