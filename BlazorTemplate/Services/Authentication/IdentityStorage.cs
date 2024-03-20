using Blazor.Infrastructure.JWT;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;

namespace BlazorTemplate.Services.Authentication;

public class IdentityStorage(ProtectedLocalStorage protectedLocalStorage)
{
    private const string KTokenStorageKey = "Token";

    private readonly ProtectedLocalStorage _protectedLocalStorage = Guard.Against.Null(protectedLocalStorage);


    public async Task PersistUserTokenAsync(AuthenticatedUserResponse authenticatedUserToken)
    {
        await _protectedLocalStorage.SetAsync(KTokenStorageKey, authenticatedUserToken);
    }


    public async Task<Result<AuthenticatedUserResponse>> GetUserTokenAsync()
    {
        ProtectedBrowserStorageResult<AuthenticatedUserResponse> result = await _protectedLocalStorage.GetAsync<AuthenticatedUserResponse>(KTokenStorageKey);

        if (result.Success && result.Value is not null)
            return Result.Ok(result.Value);
        else
            return Result.Fail("Invalid token in storage!");
    }


    public async Task ClearBrowserUserDataAsync()
    {
        await _protectedLocalStorage.DeleteAsync(KTokenStorageKey);
    }
}
