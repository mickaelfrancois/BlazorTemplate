namespace Blazor.Application.Interfaces;

public interface IGravatarService
{
    string GetUserUrlProfile(string email, int size = 32);
}