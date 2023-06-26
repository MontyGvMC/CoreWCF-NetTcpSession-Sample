using System;
using CoreWCF;
using Microsoft.Extensions.Logging;

namespace SessionNetTcpServer.Services
{

    // Define a service contract.
    [ServiceContract]
    public interface ICalculatorService
    {
        [OperationContract]
        double Add(double n1, double n2);
        [OperationContract]
        double Subtract(double n1, double n2);
        [OperationContract]
        double Multiply(double n1, double n2);
        [OperationContract]
        double Divide(double n1, double n2);
    }

    public class CalculatorService : ICalculatorService
    {

        //protected readonly ILogger<CalculatorService> Logger;

        //public CalculatorService(ILogger<CalculatorService> logger)
        //{
        //    Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //}

        public double Add(double n1, double n2)
        {
            double res = n1 + n2;
            //Logger.LogInformation("add: {N1} + {N2} = {Res}", n1, n2, res);
            return res;
        }

        public double Subtract(double n1, double n2)
        {
            double res = n1 - n2;
            //Logger.LogInformation("sub: {N1} - {N2} = {Res}", n1, n2, res);
            return res;
        }

        public double Multiply(double n1, double n2)
        {
            double res = n1 * n2;
            //Logger.LogInformation("mul: {N1} * {N2} = {Res}", n1, n2, res);
            return res;
        }

        public double Divide(double n1, double n2)
        {
            double res = n1 / n2;
            //Logger.LogInformation("div: {N1} / {N2} = {Res}", n1, n2, res);
            return res;

        }

    }

}
