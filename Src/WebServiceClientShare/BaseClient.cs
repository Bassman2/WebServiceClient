

//namespace WebServiceClient;


//public class BaseClient<T> : IDisposable where T : class, IDisposable
//{
//    private T? service;

//    public BaseClient(string storeKey, string appName)
//        : this(new Uri(KeyStore.Key(storeKey)?.Host!), KeyStore.Key(storeKey)!.Token!, appName)
//    { }

//    public BaseClient(Uri host, string token, string appName)
//    {
//        service = new(host, new ApiKeyAuthenticator("X-JFrog-Art-Api", token), appName);
//    }

//    public void Dispose()
//    {
//        if (this.service != null)
//        {
//            this.service.Dispose();
//            this.service = null;
//        }
//        GC.SuppressFinalize(this);
//    }
//}
