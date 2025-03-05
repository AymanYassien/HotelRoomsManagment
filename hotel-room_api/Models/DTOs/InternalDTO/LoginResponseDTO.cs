namespace hotel_room_api.Models.DTOs.InternalDTO;

public class LoginResponseDTO
{
    public InternalUser User { get; set; }
    public string Token { get; set; }
}