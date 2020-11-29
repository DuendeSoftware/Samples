using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ResourcesScopesConsoleClient
{
    class Program
    {
        private static DiscoveryCache Cache;
        
        static async Task Main(string[] args)
        {
            Console.Title = "Console Resources and Scopes Client";
            Cache = new DiscoveryCache("https://localhost:5001");

            var leave = false;
            
            while (leave == false)
            {
                Console.Clear();
                
                "Resource setup:\n".ConsoleGreen();

                "api1: api1.scope1 api1.scope2 shared.scope".ConsoleGreen();
                "api2: api2.scope1 api2.scope2 shared.scope\n".ConsoleGreen();
                "scopes without resource association: scope2 scope3 transaction\n\n".ConsoleGreen();
                
                
                // scopes without associated resource
                "a) scope2 scope3".ConsoleYellow();

                // one scope, single resource
                "b) api1.scope1".ConsoleYellow();
                
                // two scopes, single resources
                "c) api1.scope1 api1.scope2".ConsoleYellow();
                
                // two scopes, one has a resource, one doesn't
                "d) api1.scope1 scope2".ConsoleYellow();
                
                // two scopes, two resource
                "e) api1.scope1 api2.scope1".ConsoleYellow();
                
                // shared scope between two resources
                "f) shared.scope".ConsoleYellow();
                
                // shared scope between two resources and scope that belongs to resource
                "g) api1.scope1 shared.scope".ConsoleYellow();
                
                // parameterized scope
                "h) transaction:123".ConsoleYellow();
                
                // no scope
                "i) no scope".ConsoleYellow();
                
                "\nx) quit".ConsoleYellow();
                
                var input = Console.ReadKey();
                
                switch (input.Key)
                {
                    case ConsoleKey.A:
                        await RequestToken("scope2 scope3");
                        break;
                    
                    case ConsoleKey.B:
                        await RequestToken("api1.scope1");
                        break;
                    
                    case ConsoleKey.C:
                        await RequestToken("api1.scope1 api1.scope2");
                        break;
                    
                    case ConsoleKey.D:
                        await RequestToken("api1.scope1 scope2");
                        break;
                    
                    case ConsoleKey.E:
                        await RequestToken("api1.scope1 api2.scope1");
                        break;
                    
                    case ConsoleKey.F:
                        await RequestToken("shared.scope");
                        break;
                    
                    case ConsoleKey.G:
                        await RequestToken("api1.scope1 shared.scope");
                        break;
                    
                    case ConsoleKey.H:
                        await RequestToken("transaction:123");
                        break;
                    
                    case ConsoleKey.I:
                        await RequestToken("");
                        break;
                    
                    case ConsoleKey.X:
                        leave = true;
                        break;
                }
            }
        }
        
        

        static async Task RequestToken(string scope)
        {
            var client = new HttpClient();
            var disco = await Cache.GetAsync();

            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "resources.and.scopes",
                ClientSecret = "secret",

                Scope = scope
            });
            
            if (response.IsError) throw new Exception(response.Error);

            Console.WriteLine();
            Console.WriteLine();
            
            response.Show();
            Console.ReadLine();
        }
    }
}