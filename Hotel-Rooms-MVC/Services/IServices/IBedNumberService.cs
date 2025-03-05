namespace Hotel_Rooms_MVC.Services.IServices;

public interface IBedNumberService
{
    Task<T> GetAllAsync<T>(string token);
    Task<T> GetAsync<T>(int id, string token);
    Task<T> AddAsync<T>(BedNumberAddDTO Entity, string token);
    Task<T> UpdateAsync<T>(BedNumberUpdateDTO Entity, string token);
    Task<T> RemoveAsync<T>(int id, string token); 

}