namespace Ernaehrbar.Parts.Exceptions;

/// <summary>
/// Base exception for all Ernährbär domain exceptions.
/// </summary>
public class ErnaehrbarException : Exception
{
    public ErnaehrbarException()
    {
    }

    public ErnaehrbarException(string? message) : base(message)
    {
    }

    public ErnaehrbarException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
