namespace Jump.Attributes.Actions.Http;

public class HttpGet(string path) : Route(path, Http.HttpAction.Get);