using BlazorTemplate.Services.Preferences;

namespace BlazorTemplate.Services.Layout;

public class LayoutService(UserPreferencesService userPreferencesService)
{
    public event EventHandler? MajorUpdateOccured;
    private void OnMajorUpdateOccured() => MajorUpdateOccured?.Invoke(this, EventArgs.Empty);

    public bool IsDarkMode { get; private set; } = false;

    private UserPreferences _userPreferences = new();

    public EDarkLightMode DarkModeToggle = EDarkLightMode.System;

    private readonly UserPreferencesService _userPreferencesService = Guard.Against.Null(userPreferencesService);


    public void SetDarkMode(bool isDarkMode)
    {
        IsDarkMode = isDarkMode;
    }


    public async Task ToggleDarkMode()
    {
        IsDarkMode = !IsDarkMode;
        _userPreferences.IsDarkMode = IsDarkMode;
        await _userPreferencesService.SaveUserPreferencesAsync(_userPreferences);
        OnMajorUpdateOccured();
    }


    public Task OnSystemPreferenceChanged(bool newValue)
    {
        if (DarkModeToggle == EDarkLightMode.System)
        {
            IsDarkMode = newValue;
            OnMajorUpdateOccured();
        }
        return Task.CompletedTask;
    }


    public async Task UpdateUserPreferences(UserPreferences preferences)
    {
        _userPreferences = preferences;
        IsDarkMode = _userPreferences.IsDarkMode;

        await _userPreferencesService.SaveUserPreferencesAsync(_userPreferences);
    }


    public async Task<UserPreferences> ApplyUserPreferences(bool isDarkModeDefaultTheme)
    {
        _userPreferences = await _userPreferencesService.LoadUserPreferences();
        if (_userPreferences != null)
        {
            IsDarkMode = _userPreferences.IsDarkMode;
        }
        else
        {
            IsDarkMode = isDarkModeDefaultTheme;
            _userPreferences = new UserPreferences { IsDarkMode = IsDarkMode };
            await _userPreferencesService.SaveUserPreferencesAsync(_userPreferences);
        }

        return _userPreferences;
    }
}
