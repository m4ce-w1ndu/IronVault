using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using IronVault.Core.Models;

namespace IronVault.App.ViewModels;

public class VaultViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainViewModel;
    private readonly ObservableCollection<VaultItem> _allItems = [];
    private ObservableCollection<VaultItem> _filteredItems = [];
    private string _searchQuery = string.Empty;
    private VaultItem? _selectedItem;

    // Edit fields
    private bool _isEditing;
    private bool _isAddingNew;
    private string _editTitle = string.Empty;
    private string _editUsername = string.Empty;
    private string _editPassword = string.Empty;
    private string _editUri = string.Empty;
    private string _editNotes = string.Empty;
    private VaultItemType _editType = VaultItemType.Login;

    public ObservableCollection<VaultItem> FilteredItems
    {
        get => _filteredItems;
        set => SetProperty(ref _filteredItems, value);
    }

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            if (SetProperty(ref _searchQuery, value))
            {
                ApplyFilter();
            }
        }
    }

    public VaultItem? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (SetProperty(ref _selectedItem, value))
            {
                IsEditing = false;
                IsAddingNew = false;
                if (value != null)
                {
                    PopulateEditFields(value);
                }
            }
        }
    }

    public bool IsEditing
    {
        get => _isEditing;
        set => SetProperty(ref _isEditing, value);
    }

    public bool IsAddingNew
    {
        get => _isAddingNew;
        set => SetProperty(ref _isAddingNew, value);
    }

    public string EditTitle
    {
        get => _editTitle;
        set => SetProperty(ref _editTitle, value);
    }

    public string EditUsername
    {
        get => _editUsername;
        set => SetProperty(ref _editUsername, value);
    }

    public string EditPassword
    {
        get => _editPassword;
        set => SetProperty(ref _editPassword, value);
    }

    public string EditUri
    {
        get => _editUri;
        set => SetProperty(ref _editUri, value);
    }

    public string EditNotes
    {
        get => _editNotes;
        set => SetProperty(ref _editNotes, value);
    }

    public VaultItemType EditType
    {
        get => _editType;
        set => SetProperty(ref _editType, value);
    }

    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelEditCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand CopyUsernameCommand { get; }
    public ICommand CopyPasswordCommand { get; }
    public ICommand LogoutCommand { get; }

    public VaultViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        FilteredItems = _allItems;

        AddCommand = new RelayCommand(StartAdd);
        EditCommand = new RelayCommand(StartEdit, () => SelectedItem != null);
        SaveCommand = new AsyncRelayCommand(SaveAsync);
        CancelEditCommand = new RelayCommand(CancelEdit);
        DeleteCommand = new AsyncRelayCommand(DeleteAsync, () => SelectedItem != null);
        CopyUsernameCommand = new AsyncRelayCommand(CopyUsernameAsync);
        CopyPasswordCommand = new AsyncRelayCommand(CopyPasswordAsync);
        LogoutCommand = new AsyncRelayCommand(LogoutAsync);
    }

    public async Task LoadItemsAsync()
    {
        _allItems.Clear();
        var items = await _mainViewModel.VaultService.GetItemsAsync();
        foreach (var item in items)
        {
            _allItems.Add(item);
        }
        ApplyFilter();
        SelectedItem = _allItems.FirstOrDefault();
    }

    private void ApplyFilter()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            FilteredItems = new ObservableCollection<VaultItem>(_allItems);
        }
        else
        {
            var query = SearchQuery.ToLowerInvariant();
            var filtered = _allItems.Where(i =>
                i.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                i.Username.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                i.Uri.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                i.Notes.Contains(query, StringComparison.OrdinalIgnoreCase)
            );
            FilteredItems = new ObservableCollection<VaultItem>(filtered);
        }
    }

    private void PopulateEditFields(VaultItem item)
    {
        EditTitle = item.Title;
        EditUsername = item.Username;
        EditPassword = item.Password;
        EditUri = item.Uri;
        EditNotes = item.Notes;
        EditType = item.Type;
    }

    private void StartAdd()
    {
        SelectedItem = null;
        IsAddingNew = true;
        IsEditing = true;

        EditTitle = "New Login";
        EditUsername = string.Empty;
        EditPassword = string.Empty;
        EditUri = string.Empty;
        EditNotes = string.Empty;
        EditType = VaultItemType.Login;
    }

    private void StartEdit()
    {
        if (SelectedItem == null) return;
        IsEditing = true;
        IsAddingNew = false;
        PopulateEditFields(SelectedItem);
    }

    private void CancelEdit()
    {
        IsEditing = false;
        IsAddingNew = false;
        if (SelectedItem != null)
        {
            PopulateEditFields(SelectedItem);
        }
        else if (_allItems.Count > 0)
        {
            SelectedItem = _allItems.FirstOrDefault();
        }
    }

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(EditTitle)) return;

        if (IsAddingNew)
        {
            var newItem = new VaultItem
            {
                Title = EditTitle,
                Username = EditUsername,
                Password = EditPassword,
                Uri = EditUri,
                Notes = EditNotes,
                Type = EditType
            };
            await _mainViewModel.VaultService.AddItemAsync(newItem);
            _allItems.Add(newItem);
            SelectedItem = newItem;
        }
        else if (SelectedItem != null)
        {
            var updatedItem = new VaultItem
            {
                Id = SelectedItem.Id,
                Title = EditTitle,
                Username = EditUsername,
                Password = EditPassword,
                Uri = EditUri,
                Notes = EditNotes,
                Type = EditType
            };
            await _mainViewModel.VaultService.UpdateItemAsync(updatedItem);

            // Update in collection
            var index = _allItems.IndexOf(SelectedItem);
            if (index >= 0)
            {
                _allItems[index] = updatedItem;
            }
            SelectedItem = updatedItem;
        }

        IsEditing = false;
        IsAddingNew = false;
        ApplyFilter();
    }

    private async Task DeleteAsync()
    {
        if (SelectedItem == null) return;

        await _mainViewModel.VaultService.DeleteItemAsync(SelectedItem.Id);
        _allItems.Remove(SelectedItem);
        ApplyFilter();
        SelectedItem = _allItems.FirstOrDefault();
    }

    private async Task CopyUsernameAsync()
    {
        if (SelectedItem == null || string.IsNullOrEmpty(SelectedItem.Username)) return;
        var clipboard = Application.Current?.GetClipboard();
        if (clipboard != null)
        {
            await clipboard.SetTextAsync(SelectedItem.Username);
        }
    }

    private async Task CopyPasswordAsync()
    {
        if (SelectedItem == null || string.IsNullOrEmpty(SelectedItem.Password)) return;
        var clipboard = Application.Current?.GetClipboard();
        if (clipboard != null)
        {
            await clipboard.SetTextAsync(SelectedItem.Password);
        }
    }

    private async Task LogoutAsync()
    {
        await _mainViewModel.LogoutAsync();
    }
}

public static class ClipboardExtensions
{
    public static IClipboard? GetClipboard(this Application app)
    {
        if (app.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow?.Clipboard;
        }
        return null;
    }
}
