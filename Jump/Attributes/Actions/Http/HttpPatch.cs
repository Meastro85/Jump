namespace Jump.Attributes.Actions.Http;

public class HttpPatch(string path) : Route(path, Http.HttpAction.Patch);