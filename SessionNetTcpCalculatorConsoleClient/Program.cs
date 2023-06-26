using System.ServiceModel;
using ServiceReference1;

namespace SessionNetTcpCalculatorConsoleClient
{

    public class Program
    {

        static void Main()
        {

            Console.Title = "Session NetTcp Calculator Console Client";

            CalculatorServiceClient client = new CalculatorServiceClient
            (
                new BasicHttpBinding(BasicHttpSecurityMode.None),
                new EndpointAddress("http://localhost:5000/Calculator/basicHttp")
            );

            Random random = new Random((int)DateTime.Now.Ticks);

            double curValue = 0;
            double nextValue;
            double response;

            while (true)
            {
                nextValue = random.Next(1, 11);

                Thread.Sleep(1000);
                
                if (random.Next(2) == 0)
                {
                    response = client
                        .Add(new AddRequest(curValue, nextValue))
                        .AddResult;

                    Console.WriteLine($"add: {curValue} + {nextValue} = {response}");

                    curValue = response;
                }
                else
                {
                    response = client
                        .Subtract(new SubtractRequest(curValue, nextValue))
                        .SubtractResult;

                    Console.WriteLine($"sub: {curValue} - {nextValue} = {response}");

                    curValue = response;
                }

            }

            // Always close the client.
            client.Close();

        }

    }

}
