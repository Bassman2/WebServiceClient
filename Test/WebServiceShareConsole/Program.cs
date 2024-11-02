namespace WebServiceShareConsole
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello, World!");

            using var service = new TestService(new Uri("http://"));
            DemoModel? demo = service.GetDemo();
        }
    }
}
