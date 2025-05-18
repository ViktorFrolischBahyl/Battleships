namespace Battleships.Models.Exceptions;

public class OutsideOfDefinedPlayingField : InvalidOperationException
{
    public OutsideOfDefinedPlayingField(string message) : base(message)
    {
    }

    public OutsideOfDefinedPlayingField(string message, Exception innerException) : base(message, innerException)
    {
    }

    public OutsideOfDefinedPlayingField()
    {
    }
}