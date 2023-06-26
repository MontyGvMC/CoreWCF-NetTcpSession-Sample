using System.Runtime.Serialization;
using CoreWCF;

namespace SessionNetTcpServer.Services
{

    [DataContract]
    public class Subscription
    {
        [DataMember]
        public string Subscriber { get; set; }

        [DataMember]
        public string Topic { get; set; }
    }

    [DataContract]
    public class Update
    {
        [DataMember]
        public double Message { get; set; }
    }

    [ServiceContract(
        SessionMode = SessionMode.Required,
        CallbackContract = typeof(ICallbackServiceCallback)
    )]
    public interface ICallbackService
    {

        bool Subscribe(Subscription subscription);

        bool Unsubscribe(Subscription subscription);
    }

    [ServiceContract]
    public interface ICallbackServiceCallback
    {
        void Receive(Update update);

    }

}
