using Hotel_Rooms_MVC.Models;

namespace Hotel_Rooms_MVC.Services.IServices;

public interface IAuthService
{
    Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDto);
    Task<T> RegisterAsync<T>(RegisterRequestDTO registerRequestDto);
}