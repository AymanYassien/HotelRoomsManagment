namespace hotel_room_api.Models.DTOs;

public class IdentityLoginResponseDTO
{
    public IdentityUserDto User { get; set; }
    // public string Role { get; set; }
    public string Token { get; set; }
}