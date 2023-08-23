using Microsoft.Identity.Client;
using Microsoft.Graph;
using Azure.Identity;

namespace cmdTestAAD; 
internal class Program
{
    private const string _idClient = "";
    private const string _idTenant = "";
    static async Task Main(string[] args)
    {
        Console.WriteLine("Elija la opción de Conexión");
        Console.WriteLine("1 = Active Directory - microsoft Identity");
        Console.WriteLine("2 = Microsoft Graph");
        var resp = Console.ReadLine();
        int.TryParse(resp, out int op);
        switch(op){
            case 1: await ActiveDirectory();
                    break;
            case 2: await MicrosoftGraph();
                    break;
        }
    }

    static async Task ActiveDirectory(){
        //dotnet add package Microsoft.Identity.Client 
        string[] scopes = {"user.read"};
        var app =  PublicClientApplicationBuilder.Create(_idClient)
                    .WithAuthority(AzureCloudInstance.AzurePublic, _idTenant)
                    .WithRedirectUri("http://localhost")
                    .Build();
        var result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
        Console.WriteLine(result.AccessToken);
    }

    static async Task MicrosoftGraph() {
        //dotnet add package Microsoft.Graph
        var scopes = new[] { "User.Read" };
        // using Azure.Identity;
        var options = new InteractiveBrowserCredentialOptions
        {
            TenantId = _idTenant,
            ClientId = _idClient,
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
            // MUST be http://localhost or http://localhost:PORT
            // See https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/System-Browser-on-.Net-Core
            RedirectUri = new Uri("http://localhost"),
        };

        // https://learn.microsoft.com/dotnet/api/azure.identity.interactivebrowsercredential
        var interactiveCredential = new InteractiveBrowserCredential(options);

        var graphClient = new GraphServiceClient(interactiveCredential, scopes);
        var user = await graphClient.Me.GetAsync();
        Console.WriteLine(user?.DisplayName);
    }
}
