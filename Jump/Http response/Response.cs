namespace Jump.Http_response;

public class Response(int statusCode,string message) : IResponse
{

    private string Message { get; set; } = message;

    public int StatusCode { get; set; } = statusCode;
    public string GetResponse()
    {
        return Message;
    }
}