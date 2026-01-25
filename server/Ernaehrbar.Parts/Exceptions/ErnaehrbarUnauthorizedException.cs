namespace Ernaehrbar.Parts.Exceptions;

/// <summary>
/// Exception thrown when a user is not authorized to perform an action.
/// </summary>
public class ErnaehrbarUnauthorizedException : ErnaehrbarException
{
    public ErnaehrbarUnauthorizedException()
    {
    }

    public ErnaehrbarUnauthorizedException(string? message) : base(message)
    {
    }

    public ErnaehrbarUnauthorizedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
