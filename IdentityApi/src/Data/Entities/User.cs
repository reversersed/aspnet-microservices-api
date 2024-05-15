using Microsoft.AspNetCore.Identity;

namespace IdentityApi.src.Data.Entities;

public class User : IdentityUser<long>
{
    public string? RefreshToken { get; set; } = null;
    public DateTime? RefreshExpirationDate { get; set; } = null;
    public DateTime? LastNameChange { get; set; } = null;
    public List<string> Scopes { get; set; } = [];
    public int? CardId { get; set; } = null;
}
