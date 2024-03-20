namespace Blazor.Shared.Extensions;

public static class StringExtensions
{
    public static bool Defined(this string input)
    {
        return !string.IsNullOrWhiteSpace(input);
    }

    public static bool NotDefined(this string input)
    {
        return string.IsNullOrWhiteSpace(input);
    }
}
