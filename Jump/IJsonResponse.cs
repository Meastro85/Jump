namespace Jump.Providers;

public interface IJsonResponse
{
    int StatusCode { get; set; }
    string ToJson();
}