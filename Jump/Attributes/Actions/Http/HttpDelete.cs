﻿namespace Jump.Attributes.Actions.Http;

public class HttpDelete(string path) : Route(path, Method.DELETE);