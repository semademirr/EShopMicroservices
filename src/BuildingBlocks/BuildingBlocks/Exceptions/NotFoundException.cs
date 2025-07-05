
namespace BuildingBlocks.Exceptions;

public class NotFoundException : Exception
{
    // we will define two different constructors in order 
    // to handle not found exception. 

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found.")
    { 
    }
}
