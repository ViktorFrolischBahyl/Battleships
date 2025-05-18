namespace Battleships.Models.Exceptions;

public class AlreadyFiredAtException : InvalidOperationException
{
    public AlreadyFiredAtException(string message) : base(message)
    {
    }

    public AlreadyFiredAtException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public AlreadyFiredAtException()
    {
    }
}