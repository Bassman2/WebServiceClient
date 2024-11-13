namespace WebServiceClient.Authenticator;

public class FritzAuthenticator(string login, string password) : IAuthenticator
{
    public void Authenticate(WebService service, HttpClient client)
    {

        string? sessionId;
        string? challenge;

        using (HttpResponseMessage response = client.GetAsync("login_sid.lua").Result)
        {
            response.EnsureSuccessStatusCode();
            Stream stream = response.Content.ReadAsStreamAsync().Result;
            XDocument doc = XDocument.Load(stream);
            XElement? info = doc.FirstNode as XElement;
            sessionId = info?.Element("SID")?.Value;
            challenge = info?.Element("Challenge")?.Value;
        }

        if (sessionId == "0000000000000000")
        {
            string resp = $"{challenge}-{GetMD5Hash(challenge + "-" + password)}";
            string request = $"login_sid.lua?username={login}&response={resp}";
            using HttpResponseMessage response = client.GetAsync(request).Result;
            response.EnsureSuccessStatusCode();
            Stream stream = response.Content.ReadAsStreamAsync().Result;
            XDocument doc = XDocument.Load(stream);
            XElement? info = doc.FirstNode as XElement;
            sessionId = info?.Element("SID")?.Value;
        }
    }

    private static string GetMD5Hash(string input)
    {
        byte[] data = MD5.HashData(Encoding.Unicode.GetBytes(input));
        return data.Select(d => d.ToString("x2")).Aggregate((a, b) => a + b);

        //MD5 md5Hasher = MD5.Create();
        //byte[] data = md5Hasher.ComputeHash(Encoding.Unicode.GetBytes(input));
        //return data.Select(d => d.ToString("x2")).Aggregate((a, b) => a + b);
    }
}
