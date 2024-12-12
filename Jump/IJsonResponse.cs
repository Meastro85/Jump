namespace Jump;

public interface IJsonResponse
{
    int StatusCode { get; set; }
    string ToJson();
}