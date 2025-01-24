namespace Jump.Attributes.Actions.Http;

public class HttpPut(string path) : Route(path, Http.HttpAction.Put);