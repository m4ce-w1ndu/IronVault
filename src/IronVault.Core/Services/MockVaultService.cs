using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IronVault.Core.Models;

namespace IronVault.Core.Services;

public class MockVaultService : IVaultService
{
    private readonly List<VaultItem> _items = [];
    private bool _isLocked = true;

    public bool IsLocked => _isLocked;

    public MockVaultService()
    {
        InitializeMockData();
    }

    public async Task<bool> UnlockAsync(string masterPassword)
    {
        // Simple mock unlock behavior: any password that is at least 6 characters and matches "password" or "ironvault"
        await Task.Delay(800); // Simulate network or crypto delay
        if (masterPassword == "password" || masterPassword == "ironvault")
        {
            _isLocked = false;
            return true;
        }
        return false;
    }

    public Task LockAsync()
    {
        _isLocked = true;
        return Task.CompletedTask;
    }

    public Task<IEnumerable<VaultItem>> GetItemsAsync()
    {
        if (_isLocked)
        {
            throw new InvalidOperationException("Vault is locked.");
        }
        return Task.FromResult<IEnumerable<VaultItem>>(_items);
    }

    public Task AddItemAsync(VaultItem item)
    {
        _items.Add(item);
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        return Task.CompletedTask;
    }

    public Task UpdateItemAsync(VaultItem item)
    {
        var existing = _items.FirstOrDefault(i => i.Id == item.Id);
        if (existing != null)
        {
            existing.Title = item.Title;
            existing.Username = item.Username;
            existing.Password = item.Password;
            existing.Uri = item.Uri;
            existing.Notes = item.Notes;
            existing.Type = item.Type;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        return Task.CompletedTask;
    }

    public Task DeleteItemAsync(Guid id)
    {
        _items.RemoveAll(i => i.Id == id);
        return Task.CompletedTask;
    }

    private void InitializeMockData()
    {
        _items.Add(new VaultItem
        {
            Title = "GitHub",
            Type = VaultItemType.Login,
            Username = "octocat@github.com",
            Password = "ghp_secureToken1234567890",
            Uri = "https://github.com/login",
            Notes = "Personal GitHub account."
        });

        _items.Add(new VaultItem
        {
            Title = "Google Account",
            Type = VaultItemType.Login,
            Username = "developer.agent@gmail.com",
            Password = "SuperSecureGooglePassword2026!",
            Uri = "https://accounts.google.com",
            Notes = "Used for development and testing."
        });

        _items.Add(new VaultItem
        {
            Title = "Netflix",
            Type = VaultItemType.Login,
            Username = "bingewatcher@netflix.com",
            Password = "Chill&StreamPasswords99!",
            Uri = "https://netflix.com/login",
            Notes = "Shared family account. Premium plan."
        });

        _items.Add(new VaultItem
        {
            Title = "Development Server SSH Key passphrase",
            Type = VaultItemType.SecureNote,
            Notes = "Passphrase for id_ed25519 is: 'antigravity-levitation-passphrase-2026'"
        });

        _items.Add(new VaultItem
        {
            Title = "Amazon Web Services",
            Type = VaultItemType.Login,
            Username = "aws-root-admin",
            Password = "mfa-enabled-complex-pwd-991",
            Uri = "https://aws.amazon.com/console",
            Notes = "Production environment access. Check authenticator for MFA token."
        });
    }
}
