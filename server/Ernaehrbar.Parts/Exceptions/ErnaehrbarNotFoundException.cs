namespace Ernaehrbar.Parts.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found.
/// </summary>
public class ErnaehrbarNotFoundException : ErnaehrbarException
{
    public ErnaehrbarNotFoundException()
    {
    }

    public ErnaehrbarNotFoundException(string? message) : base(message)
    {
    }

    public ErnaehrbarNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
