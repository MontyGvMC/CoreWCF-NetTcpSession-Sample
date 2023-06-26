using System;
using System.Runtime.Serialization;
using CoreWCF;
using Microsoft.Extensions.Logging;

namespace SessionNetTcpServer.Services
{

    /// <summary>
    /// Response from the service.
    /// </summary>
    [DataContract]
    public class SpecialData
    {

        /// <summary>
        /// GUID of the service instance responding.
        /// </summary>
        [DataMember]
        public string GUID { get; set; }

        /// <summary>
        /// The current calculated value of the session.
        /// </summary>
        [DataMember]
        public int Value { get; set; }

    }

    /// <summary>
    /// Contract of the service requiring a session.
    /// </summary>
    [ServiceContract(
        SessionMode = SessionMode.Required
    )]
    public interface ISpecialService
    {

        /// <summary>
        /// Increments the current value of the session by one.
        /// </summary>
        /// <returns>Returns the current value of the session and the session GUID.</returns>
        [OperationContract]
        SpecialData Increment();

        /// <summary>
        /// Decrements the current value of the session by one.
        /// </summary>
        /// <returns>Returns the current value of the session and the session GUID.</returns>
        [OperationContract]
        SpecialData Decrement();

    }

    /// <summary>
    /// Implementation of the service requiring a session.
    /// </summary>
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerSession,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        AddressFilterMode = AddressFilterMode.Exact
        //AutomaticSessionShutdown = true,
        //UseSynchronizationContext = false     //https://github.com/CoreWCF/CoreWCF/issues/58
    )]
    public class SpecialService : ISpecialService
    {

        /// <summary>
        /// The session GUID assigned to this instance.
        /// </summary>
        protected readonly string GUID = Guid.NewGuid().ToString();

        /// <summary>
        /// The current value of the session.
        /// </summary>
        protected int Value { get; set; }

        /// <summary>
        /// The logger to be used for logging.
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SpecialService(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
            Logger = loggerFactory.CreateLogger(GetType().FullName + "[" + GUID + "]");
        }

        /// <inheritdoc/>
        public SpecialData Increment()
        {
            Value++;

            Logger.LogInformation("increment to {Value}", Value);

            return new SpecialData
            {
                GUID  = GUID,
                Value = Value
            };
        }

        /// <inheritdoc/>
        public SpecialData Decrement()
        {
            Value--;

            Logger.LogInformation("decrement to {Value}", Value);

            return new SpecialData
            {
                GUID  = GUID,
                Value = Value
            };
        }

    }

}
