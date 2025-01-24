namespace Jump.Http_response;

public interface IResponse
{
    int StatusCode { get; set; }
    string GetResponse();
}