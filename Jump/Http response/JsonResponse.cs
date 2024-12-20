using System.Text.Json;

namespace Jump.Http_response;

public class JsonResponse<T> : IJsonResponse
{
    public T Data { get; set; }
    public int StatusCode { get; set; }

    public JsonResponse(T data, int statusCode = 200)
    {
        Data = data;
        StatusCode = statusCode;
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(Data);
    }
    
}