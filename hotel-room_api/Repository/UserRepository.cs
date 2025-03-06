using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using hotel_room_api.Data;
using hotel_room_api.Models;
using hotel_room_api.Models.DTOs;
using hotel_room_api.Models.DTOs.InternalDTO;
using hotel_room_api.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace hotel_room_api.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    
    private  string secretKey;

    public UserRepository(AppDbContext db, IConfiguration configuration,
        RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
        secretKey = configuration.GetValue<string>("ApiSettings:Secret");
    }


    public async Task<bool> IsUniqueUserName(string userName)
    {
        //var user = await _db.InternalUsers.FirstOrDefaultAsync(u => u.UserName == userName);
        var user = await _db.AppUsers.FirstOrDefaultAsync(u => u.UserName == userName);
        
        return (user == null);
    }
    

    public async Task<IdentityLoginResponseDTO> Login(LoginRequestDTO loginRequestDto)
    {
        
        // User Identity 
        var userIdentity = _db.AppUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

        bool isValidPwd = await _userManager.CheckPasswordAsync(userIdentity, loginRequestDto.Password);
        
        if (userIdentity == null || isValidPwd == false)
        {
            return new IdentityLoginResponseDTO()
            {
                Token ="",
                User = null
            };
        }

        var roles = await _userManager.GetRolesAsync(userIdentity);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        var TokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userIdentity.UserName),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault())
            }),
            Expires = DateTime.UtcNow.AddDays(15),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        IdentityUserDto userDto = new IdentityUserDto()
        {
            Name = userIdentity.Name,
            userName = userIdentity.UserName,
            ID = userIdentity.Id
        };

        var token = tokenHandler.CreateToken(TokenDescriptor);
        IdentityLoginResponseDTO loginResponseDto = new IdentityLoginResponseDTO()
        {
            Token = tokenHandler.WriteToken(token),
            // Role = roles.FirstOrDefault(),
            User = userDto
        };

        return loginResponseDto;
    }
    
    public async Task<LoginResponseDTO> LoginHardWay(LoginRequestDTO loginRequestDto)
    {
        // Hard way
        var user =await _db.InternalUsers.FirstOrDefaultAsync(
            u => u.UserName.ToLower() == loginRequestDto.UserName
                 && u.Password == loginRequestDto.Password);
        
        if (user == null)
        {
            return new LoginResponseDTO()
            {
                Token ="",
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

    public async Task<IdentityUserDto> Register(RegisterRequestDTO registerRequestDto)
    {
        AppUser user = new AppUser()
        {
            UserName = registerRequestDto.UserName,
            Email=registerRequestDto.UserName,
            NormalizedEmail=registerRequestDto.UserName.ToUpper(),
            Name = registerRequestDto.Name,
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
            if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole("admin"));
                await _roleManager.CreateAsync(new IdentityRole("test"));
            }
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "admin");
                var userToReturn =
                    await _db.AppUsers.FirstOrDefaultAsync(u => u.UserName == registerRequestDto.UserName);
                return new IdentityUserDto()
                {
                    ID = userToReturn.Id,
                    Name = userToReturn.Name,
                    userName = userToReturn.UserName
                };
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return  null;
    }
    
    public async Task<InternalUser> RegisterHardWay(RegisterRequestDTO registerRequestDto)
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