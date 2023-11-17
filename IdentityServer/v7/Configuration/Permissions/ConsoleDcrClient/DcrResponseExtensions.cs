using System.Text.Json;
using IdentityModel.Client;

namespace ConsoleDcrClient;

public static class DcrResponseExtensions
{
    public static void Show(this DynamicClientRegistrationResponse response)
    {
        Console.WriteLine(JsonSerializer.Serialize(new 
        {
            response.ClientId,
            response.ClientSecret
        }, new JsonSerializerOptions { WriteIndented = true }));

    }
}
