using Hotel_Rooms_MVC.Services.IServices;
using RoomsUtility;

namespace Hotel_Rooms_MVC.Services.Services;

public class RoomService : BaseService, IRoomService
{
    private readonly IHttpClientFactory _clientFactory;
    private string _roomUrl;
    public RoomService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
    {
        _clientFactory = httpClient;
        _roomUrl = configuration.GetValue<string>("ServiceUrls:RoomApi");
    }

    public Task<T> AddAsync<T>(RoomCreateDTO Entity, string token)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticData.ApiTypes.POST,
            data = Entity,
            Url = _roomUrl + "/api/RoomApi",
            token = token
        });
    }
    
    public Task<T> GetAllAsync<T>(string token)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticData.ApiTypes.GET,
            Url = _roomUrl + "/api/RoomApi",
            token = token
        });
    }

    public Task<T> GetAsync<T>(int id, string token)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticData.ApiTypes.GET,
            Url = _roomUrl + "/api/RoomApi/" + id,
            token = token
        });
    }

    public Task<T> UpdateAsync<T>(RoomUpdateDTO Entity, string token)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticData.ApiTypes.PUT,
            data = Entity,
            Url = _roomUrl + "/api/RoomApi/" + Entity.Id,
            token = token
        });
    }
    
    public Task<T> RemoveAsync<T>(int id, string token)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticData.ApiTypes.DELETE,
            Url = _roomUrl + "/api/RoomApi/" + id,
            token = token
        });
    }
}