using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Cryptography;

namespace BlazorTemplate.Services.Preferences;


public class UserPreferencesService(ProtectedLocalStorage protectedLocalStorage)
{
    private const string KUserPreferencesStorageKey = "UserPreferencesService";

    private readonly ProtectedLocalStorage _protectedLocalStorage = Guard.Against.Null(protectedLocalStorage);


    public async Task<UserPreferences> LoadUserPreferences()
    {
        try
        {
            var result = await _protectedLocalStorage.GetAsync<UserPreferences>(KUserPreferencesStorageKey);

            if (result.Success && result.Value is not null)
                return result.Value;

            return new UserPreferences();
        }
        catch (CryptographicException)
        {
            await _protectedLocalStorage.DeleteAsync(KUserPreferencesStorageKey);
            return new UserPreferences();
        }
    }


    public async Task SaveUserPreferencesAsync(UserPreferences userPreferences)
    {
        await _protectedLocalStorage.SetAsync(KUserPreferencesStorageKey, userPreferences);
    }
}
