namespace Jump.Attributes.Actions.Http;

public class HttpPost(string path) : Route(path, Http.HttpAction.Post);