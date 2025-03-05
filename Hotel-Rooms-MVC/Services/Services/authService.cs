using Hotel_Rooms_MVC.Models;
using Hotel_Rooms_MVC.Services.IServices;
using RoomsUtility;

namespace Hotel_Rooms_MVC.Services.Services;

public class authService :BaseService, IAuthService
{
    private readonly IHttpClientFactory _clientFactory;
    private string _userUrl;
    public authService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
    {
        _clientFactory = httpClient;
        _userUrl = configuration.GetValue<string>("ServiceUrls:RoomApi");
    }

    public Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDto)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticData.ApiTypes.POST,
            data = loginRequestDto,
            Url = _userUrl + "/api/UserAuth/Login"
        });
    }

    public Task<T> RegisterAsync<T>(RegisterRequestDTO registerRequestDto)
    {
        return SendAsync<T>(new ApiRequest()
        {
            ApiType = StaticData.ApiTypes.POST,
            data = registerRequestDto,
            Url = _userUrl + "/api/UserAuth/register"
        });
    }
}