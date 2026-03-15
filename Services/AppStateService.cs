using Microsoft.JSInterop;
using System.Text.Json;

namespace NorthEndOneEntrance.Services;

public class AppStateService
{
    private readonly IJSRuntime? _jsRuntime;
    private Branch? _selectedBranch;
    private bool _isAuthenticated;
    private bool _isInitialized;

    public event Action? OnChange;

    public AppStateService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        if (_isInitialized || _jsRuntime == null) return;
        
        try
        {
            // Load authentication state
            var authJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "isAuthenticated");
            if (!string.IsNullOrEmpty(authJson))
            {
                _isAuthenticated = bool.Parse(authJson);
            }

            // Load selected branch
            var branchJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "selectedBranch");
            if (!string.IsNullOrEmpty(branchJson))
            {
                _selectedBranch = JsonSerializer.Deserialize<Branch>(branchJson);
            }

            _isInitialized = true;
            NotifyStateChanged();
        }
        catch
        {
            // Ignore errors during prerendering
        }
    }

    public Branch? SelectedBranch
    {
        get => _selectedBranch;
        set
        {
            _selectedBranch = value;
            _ = SaveToLocalStorageAsync();
            NotifyStateChanged();
        }
    }

    public bool IsAuthenticated
    {
        get => _isAuthenticated;
        set
        {
            _isAuthenticated = value;
            _ = SaveToLocalStorageAsync();
            NotifyStateChanged();
        }
    }

    private async Task SaveToLocalStorageAsync()
    {
        if (_jsRuntime == null) return;

        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "isAuthenticated", _isAuthenticated.ToString());
            
            if (_selectedBranch != null)
            {
                var branchJson = JsonSerializer.Serialize(_selectedBranch);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "selectedBranch", branchJson);
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "selectedBranch");
            }
        }
        catch
        {
            // Ignore errors during prerendering
        }
    }

    private void NotifyStateChanged() => OnChange?.Invoke();

    public async Task ResetAsync()
    {
        _selectedBranch = null;
        _isAuthenticated = false;
        
        if (_jsRuntime != null)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "isAuthenticated");
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "selectedBranch");
            }
            catch
            {
                // Ignore errors
            }
        }
        
        NotifyStateChanged();
    }
}
