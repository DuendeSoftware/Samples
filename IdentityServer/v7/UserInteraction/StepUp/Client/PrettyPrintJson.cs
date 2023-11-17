using System.Text.Json;

namespace Client;

public static class PrettyPrintJsonExtensions
{
    public static string PrettyPrintJson(this string raw)
    {
        var doc = JsonDocument.Parse(raw).RootElement;
        return JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
    }
}