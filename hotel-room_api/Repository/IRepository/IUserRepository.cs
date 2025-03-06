using hotel_room_api.Models;
using hotel_room_api.Models.DTOs;
using hotel_room_api.Models.DTOs.InternalDTO;

namespace hotel_room_api.Repository.IRepository;

public interface IUserRepository
{
    Task<bool> IsUniqueUserName(string userName);
    Task<IdentityLoginResponseDTO> Login(LoginRequestDTO loginRequestDto);
    Task<LoginResponseDTO> LoginHardWay(LoginRequestDTO loginRequestDto);
    Task<IdentityUserDto> Register(RegisterRequestDTO registerRequestDto);
    Task<InternalUser> RegisterHardWay(RegisterRequestDTO registerRequestDto);


}