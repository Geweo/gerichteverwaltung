namespace Ernaehrbar.Parts.Exceptions;

/// <summary>
/// Exception thrown when validation fails.
/// </summary>
public class ErnaehrbarValidationException : ErnaehrbarException
{
    public ErnaehrbarValidationException()
    {
    }

    public ErnaehrbarValidationException(string? message) : base(message)
    {
    }

    public ErnaehrbarValidationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
