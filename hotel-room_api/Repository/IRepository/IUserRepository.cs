using hotel_room_api.Models;
using hotel_room_api.Models.DTOs.InternalDTO;

namespace hotel_room_api.Repository.IRepository;

public interface IUserRepository
{
    Task<bool> IsUniqueUserName(string userName);
    Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDto);
    Task<InternalUser> Register(RegisterRequestDTO registerRequestDto);


}