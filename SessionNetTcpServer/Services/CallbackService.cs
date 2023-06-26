using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Timers;
using CoreWCF;
using Microsoft.Extensions.Logging;

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
        public string Topic { get; set; }

        [DataMember]
        public string Message { get; set; }
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

    internal class Receiver
    {
        internal readonly string Subscriber;
        internal ICallbackServiceCallback Callback;

        internal Receiver(string subscriber, ICallbackServiceCallback callback)
        {
            Subscriber = subscriber;
            Callback = callback;
        }
    }

    public class CallbackService : ICallbackService
    {

        protected readonly object Lock = new();

        protected readonly ILogger Logger;

        protected Timer TopicTimer;

        internal readonly Dictionary<string, List<Receiver>> Subscriptions = new();


        public CallbackService(ILogger<CallbackService> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            TopicTimer = new Timer();
            TopicTimer.Interval = 2000;
            TopicTimer.Elapsed += TopicTimer_Elapsed;
            TopicTimer.Start();
        }

        private void TopicTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (Lock)
            {
                TopicTimer.Stop();

                try
                {

                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "error in TopicTimer_Elapsed");
                }

                TopicTimer.Start();
            }
        }

        public bool Subscribe(Subscription subscription)
        {
            lock (Lock)
            {
                try
                {
                    if (Subscriptions.TryGetValue(subscription.Topic, out List<Receiver> receivers))
                    {
                        receivers.Add
                        (
                            new Receiver
                            (
                                subscription.Subscriber,
                                OperationContext.Current.GetCallbackChannel<ICallbackServiceCallback>()
                            )
                        );
                    }
                    else
                    {
                        Subscriptions.Add
                        (
                            subscription.Topic, 
                            new List<Receiver>() 
                            {
                                new Receiver
                                (
                                    subscription.Subscriber,
                                    OperationContext.Current.GetCallbackChannel<ICallbackServiceCallback>()
                                )
                            }
                        );
                    }

                    Logger.LogInformation("{Subscriber} subscribed for {Topic}", subscription.Subscriber, subscription.Topic);
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "error in subscribe");
                    return false;
                }
            }
        }

        public bool Unsubscribe(Subscription subscription)
        {
            lock (Lock)
            {
                try
                {
                    if (Subscriptions.TryGetValue(subscription.Topic, out List<Receiver> receivers))
                    {
                        Receiver receiver = receivers.FirstOrDefault(r => r.Subscriber.Equals(subscription.Subscriber));
                        if (receiver != null)
                        {
                            receivers.Remove(receiver);
                        }
                    }

                    Logger.LogInformation("{Subscriber} unsubscribed from {Topic}", subscription.Subscriber, subscription.Topic);
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "error in unsubscribe");
                    return false;
                }
            }
        }

    }

}