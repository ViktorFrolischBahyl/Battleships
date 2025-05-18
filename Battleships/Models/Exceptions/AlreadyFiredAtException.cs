namespace Battleships.Models.Exceptions;

/// <summary>
/// Exception thrown when a player tries to fire at a cell that has already been fired at.
/// </summary>
/// <seealso cref="System.InvalidOperationException" />
public class AlreadyFiredAtException : InvalidOperationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AlreadyFiredAtException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public AlreadyFiredAtException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AlreadyFiredAtException"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference (<see langword="Nothing" /> in Visual Basic), the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public AlreadyFiredAtException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AlreadyFiredAtException"/> class.
    /// </summary>
    public AlreadyFiredAtException()
    {
    }
}