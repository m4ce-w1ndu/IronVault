using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IronVault.Core.Models;

namespace IronVault.Core.Services;

public interface IVaultService
{
    bool IsLocked { get; }
    Task<bool> UnlockAsync(string masterPassword);
    Task LockAsync();
    Task<IEnumerable<VaultItem>> GetItemsAsync();
    Task AddItemAsync(VaultItem item);
    Task UpdateItemAsync(VaultItem item);
    Task DeleteItemAsync(Guid id);
}
