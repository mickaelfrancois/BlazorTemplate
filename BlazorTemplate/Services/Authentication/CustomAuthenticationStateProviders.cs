using Blazor.Application.Users.Queries.GetUserById;
using Blazor.Application.Users.Queries.GetUserWithCredentials;
using Blazor.Infrastructure.Extensions;
using Blazor.Infrastructure.JWT;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BlazorTemplate.Services.Authentication;

public class CustomAuthenticationStateProviders(ISender mediator,
                                                IdentityStorage identityStorage,
                                                IJwtLoginService jwtLoginService,
                                                TokenValidatorService tokenValidator,
                                                ILogger<CustomAuthenticationStateProviders> logger) : AuthenticationStateProvider
{
    private readonly ILogger _logger = Guard.Against.Null(logger);

    private readonly ISender _mediator = Guard.Against.Null(mediator);

    private readonly TokenValidatorService _tokenValidator = Guard.Against.Null(tokenValidator);

    private readonly IJwtLoginService _jwtLoginService = Guard.Against.Null(jwtLoginService);

    private readonly IdentityStorage _identityStorage = Guard.Against.Null(identityStorage);

    public UserVm CurrentUser { get; private set; } = new();

    public string? AccessToken { get; private set; }

    public string? RefreshToken { get; private set; }


    public async Task<Result> LoginAsync(string username, string password)
    {
        ClaimsPrincipal principal = new();
        UserDto? resultUser = await _mediator.Send(new GetUserWithCredentialsQuery(username, password));

        string errorMessage = "";

        if (resultUser is not null)
        {
            if (resultUser.LockoutEnabled)
            {
                errorMessage = "Account is disabled!";
            }
            else
            {
                Result<UserVm> user = await _mediator.Send(new GetUserByIdQuery(resultUser.Id));
                principal = UserToClaimsPrincipal(user.Value);
                SetUserPropertiesFromClaimsPrincipal(principal);

                AuthenticatedUserResponse userAccessToken = _jwtLoginService.GetToken(principal);
                await _identityStorage.PersistUserTokenAsync(userAccessToken);
            }
        }
        else
            errorMessage = "Please check your username and password. If you are still unable to log in, contact an administrator.";

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));


        if (errorMessage.Defined())
        {
            _logger.LogError("User '{username}' failed authentication: {errorMessage}", username, errorMessage);
            return Result.Fail(errorMessage);
        }
        else
        {
            _logger.LogInformation("User '{username}' has successfully logged in", username);
            return Result.Ok();
        }
    }


    public async Task LogoutAsync()
    {
        await _identityStorage.ClearBrowserUserDataAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
    }


    public async Task<ClaimsPrincipal> ParseClaimsFromJwt(string? accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
            return new ClaimsPrincipal(new ClaimsIdentity());

        Result<ClaimsIdentity> validationResult = await _tokenValidator.ValidateAccessTokenAsync(accessToken);
        if (validationResult.IsSuccess)
            return SetUserPropertiesFromClaimsPrincipal(new ClaimsPrincipal(validationResult.Value));

        return new ClaimsPrincipal(new ClaimsIdentity());
    }


    private ClaimsPrincipal SetUserPropertiesFromClaimsPrincipal(ClaimsPrincipal principal)
    {
        CurrentUser.Id = principal.GetUserId();
        CurrentUser.Name = principal.GetUserName()!;
        CurrentUser.Email = principal.GetEmail()!;

        return principal;
    }


    private async Task<ClaimsPrincipal> GetClaimsPrincipal()
    {
        try
        {
            Result<AuthenticatedUserResponse> resultToken = await _identityStorage.GetUserTokenAsync();

            if (resultToken.IsSuccess)
            {
                AccessToken = resultToken.Value.AccessToken;
                RefreshToken = resultToken.Value.RefreshToken;

                Result<ClaimsIdentity> validationResult = await _tokenValidator.ValidateAccessTokenAsync(AccessToken!);
                if (validationResult.IsSuccess)
                    return SetUserPropertiesFromClaimsPrincipal(new ClaimsPrincipal(validationResult.Value));

                var validationRefreshResult = await _tokenValidator.ValidateRefreshTokenAsync(RefreshToken!);
                if (validationRefreshResult.IsSuccess)
                    return SetUserPropertiesFromClaimsPrincipal(new ClaimsPrincipal(validationRefreshResult.Value));
            }
        }
        catch (CryptographicException)
        {
            await _identityStorage.ClearBrowserUserDataAsync();
        }
        catch (Exception)
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        return new ClaimsPrincipal(new ClaimsIdentity());
    }


    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

        return new AuthenticationState(claimsPrincipal);
    }


    private static ClaimsPrincipal UserToClaimsPrincipal(UserVm user)
    {
        var claims = new Claim[]
            {
                new (ClaimTypes.Name, user.Name),
                new (ClaimTypes.Sid, user.Id.ToString()),
                new (ClaimTypes.Email, user.Email.ToString()),
            }.Concat(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)).ToArray());

        return new(new ClaimsIdentity(claims, "CustomAuth"));
    }
}