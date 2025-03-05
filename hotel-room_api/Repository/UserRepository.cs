using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using hotel_room_api.Data;
using hotel_room_api.Models;
using hotel_room_api.Models.DTOs.InternalDTO;
using hotel_room_api.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace hotel_room_api.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    private  string secretKey;

    public UserRepository(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        secretKey = configuration.GetValue<string>("ApiSettings:Secret");
    }


    public async Task<bool> IsUniqueUserName(string userName)
    {
        var user = await _db.InternalUsers.FirstOrDefaultAsync(u => u.UserName == userName);
        
        return (user == null);
    }

    public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDto)
    {
        var user =await _db.InternalUsers.FirstOrDefaultAsync(
            u => u.UserName.ToLower() == loginRequestDto.UserName
                 && u.Password == loginRequestDto.Password);
        if (user == null)
        {
            return new LoginResponseDTO()
            {
                Token =" tokenHandler.WriteToken(token)",
                User = null
            };
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        var TokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(15),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(TokenDescriptor);
        LoginResponseDTO loginResponseDto = new LoginResponseDTO()
        {
            Token = tokenHandler.WriteToken(token),
            User = user
        };

        return loginResponseDto;
    }

    public async Task<InternalUser> Register(RegisterRequestDTO registerRequestDto)
    {
        InternalUser user = new InternalUser()
        {
            UserName = registerRequestDto.UserName,
            Password = registerRequestDto.Password,
            Name = registerRequestDto.Name,
            Role = registerRequestDto.Role
        };
        _db.InternalUsers.Add(user);
        await _db.SaveChangesAsync();

        user.Password = "";
        return user;
    }
}