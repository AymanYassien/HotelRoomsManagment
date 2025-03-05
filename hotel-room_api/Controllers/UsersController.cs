using System.Net;
using hotel_room_api.Models;
using hotel_room_api.Models.DTOs.InternalDTO;
using hotel_room_api.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace hotel_room_api.Controllers;

[Route("api/UserAuth")]
[ApiController]
public class UsersController : Controller
{
    private readonly IUserRepository _userRepository;
    protected APIResponse _apiResponse;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _apiResponse = new APIResponse();
    }

    [HttpPost("login")]
    public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequestDTO user)
    {
        var LoginResponse = await _userRepository.Login(user);
        if (LoginResponse.User == null || string.IsNullOrEmpty(LoginResponse.Token) )
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            _apiResponse.ErrorMessages = new List<string>() { "Username or Password is not Valid" };
            
            return BadRequest(_apiResponse);
        }
        
        _apiResponse.IsSuccess = true;
        _apiResponse.StatusCode = HttpStatusCode.OK;
        _apiResponse.Result = LoginResponse;
        return Ok(_apiResponse);
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<APIResponse>> Register([FromBody] RegisterRequestDTO user)
    {
        try
        {
            bool isUserNameUnique = await _userRepository.IsUniqueUserName(user.UserName);

            if (!isUserNameUnique)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages = new List<string>() { "Username Already Exist" };

                return BadRequest(_apiResponse);

            }

            var registerdUser = await _userRepository.Register(user);
            if (registerdUser == null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages = new List<string>() { "Registration Failed" };

                return BadRequest(_apiResponse);
            }

            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Result = registerdUser;
            return Ok(_apiResponse);
        }
        catch (Exception e)
        {
            _apiResponse.IsSuccess = false;
            _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
            _apiResponse.ErrorMessages = new List<string>() { e.Message };
            return BadRequest(_apiResponse);
        }
    }
}









