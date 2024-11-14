namespace Jump.Exceptions;

public class TooManyAttributesException : Exception
{
    public TooManyAttributesException()
    {
    }

    public TooManyAttributesException(string message) : base(message)
    {
    }
    
}