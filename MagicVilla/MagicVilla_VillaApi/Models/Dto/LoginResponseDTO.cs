namespace MagicVilla_VillaApi.Models.Dto;

public class LoginResponseDTO
{
    public LocalUser User { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}
