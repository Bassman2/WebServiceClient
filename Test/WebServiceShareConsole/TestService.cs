using WebServiceClient;

namespace WebServiceShareConsole;

public class TestService(Uri host, IAuthenticator? authenticator = null) 
    : JsonService(host, authenticator, "Test", SourceGenerationContext.Default)
{
    //public DemoModel? GetDemo() => GetFromJson<DemoModel>("/demo/xyz");
}
