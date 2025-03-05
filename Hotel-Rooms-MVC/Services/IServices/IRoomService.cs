
namespace Hotel_Rooms_MVC.Services.IServices;

public interface IRoomService 
{
    Task<T> GetAllAsync<T>(string token);
    Task<T> GetAsync<T>(int id, string token);
    Task<T> AddAsync<T>(RoomCreateDTO Entity, string token);
    Task<T> UpdateAsync<T>(RoomUpdateDTO Entity, string token);
    Task<T> RemoveAsync<T>(int id, string token); 
    
}