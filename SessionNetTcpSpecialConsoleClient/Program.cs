using System.ServiceModel;
using ServiceReference2;

namespace SessionNetTcpSpecialConsoleClient
{
    public class Program
    {

        static void Main()
        {

            Console.Title = "Session NetTcp Special Console Client";

            SpecialServiceClient client = new SpecialServiceClient
            (
                new BasicHttpBinding(BasicHttpSecurityMode.None),
                new EndpointAddress("http://localhost:5000/Special/basicHttp")
            );

            //SpecialServiceClient client = new SpecialServiceClient
            //(
            //    new NetTcpBinding(SecurityMode.None, true),
            //    new EndpointAddress("http://localhost:8088/Special/netTcp")
            //);

            Random random = new Random((int)DateTime.Now.Ticks);

            while (true)
            {
                
                Thread.Sleep(1000);

                if (random.Next(2) == 0)
                {
                    var res = client.Increment(new IncrementRequest()).IncrementResult;
                    Console.WriteLine($"[{res.GUID}] incr to {res.Value}");
                }
                else
                {
                    var res = client.Decrement(new DecrementRequest()).DecrementResult;
                    Console.WriteLine($"[{res.GUID}] decr to {res.Value}");
                }

            }

            // Always close the client.
            client.Close();

        }

    }
}
