using System.ComponentModel.DataAnnotations;

namespace Hotel_Rooms_MVC;

public class RoomDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    [MaxLength(30)]
    public string  Name { get; set; }
    public string  Details { get; set; }
    [Required]
    public int  Rate { get; set; }
    public int SpaceByMiter { get; set; }
    public int NumberOfBeds { get; set; } 
    public string?  ImageUrl { get; set; }
    public string  Amenity { get; set; }

}