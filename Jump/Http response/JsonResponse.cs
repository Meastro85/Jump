using System.Text.Json;

namespace Jump.Http_response;

public class JsonResponse<T> : IResponse
{
    public JsonResponse(T data, int statusCode = 200)
    {
        Data = data;
        StatusCode = statusCode;
    }

    private T Data { get; set; }
    public int StatusCode { get; set; }

    public string GetResponse()
    {
        return JsonSerializer.Serialize(Data);
    }
}