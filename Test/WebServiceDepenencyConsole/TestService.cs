using WebServiceClient;
using WebServiceClient.Authenticator;

namespace WebServiceDepenencyConsole;

public class TestService(Uri host, IAuthenticator? authenticator = null) 
    : JsonService(host, SourceGenerationContext.Default, authenticator)
{
    public DemoModel? GetDemo() => GetFromJson<DemoModel>("/demo/xyz");

}
