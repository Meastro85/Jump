namespace Jump.Http_response;

public interface IJsonResponse
{
    int StatusCode { get; set; }
    string ToJson();
}