Console.OutputEncoding = Encoding.UTF8;

var responseObject = new ResponseObject { Id = Guid.NewGuid(), Error = "Ошибка на русском" };

// More info about Encoder
// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/character-encoding#serialize-all-characters

var jsonSerializerOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
};

var serializedNonEscapedJson = JsonSerializer.Serialize(responseObject, jsonSerializerOptions);

Console.WriteLine($"Serialized non escaped json : {serializedNonEscapedJson}");

var jsonSerializerOptionsDefault = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Encoder = JavaScriptEncoder.Default,
};

var serializedEscapedJson = JsonSerializer.Serialize(responseObject, jsonSerializerOptionsDefault);

Console.WriteLine($"Serialized escaped json : {serializedEscapedJson}");