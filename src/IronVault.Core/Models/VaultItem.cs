using System;

namespace IronVault.Core.Models;

public enum VaultItemType
{
    Login,
    Card,
    Identity,
    SecureNote
}

public class VaultItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public VaultItemType Type { get; set; } = VaultItemType.Login;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
