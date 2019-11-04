using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace SBFanOut
{
    public static class Function1
    {
        // Can be 
        [FunctionName("Function1")]
        public static void Run(
            [ServiceBusTrigger("topic-a", "topic-a-sub-a", 
            Connection = "sbctx", IsSessionsEnabled = false)]string mySbMsg, 
            int deliveryCount, 
            DateTime expiresAtUtc,
            DateTime enqueuedTimeUtc, 
            string messageId,
            string contentType,
            string replyTo,
            long sequenceNumber,
            string to,
            string label,
            string correlationId,
            ExecutionContext exCtx,
            ILogger log)
        {
            var data = new
            {
                Message = mySbMsg,
                DeliveryCount = deliveryCount,
                ExpiresAtUtc = expiresAtUtc,
                EnqueuedTimeUtc = enqueuedTimeUtc,
                MessageId = messageId,
                ContentType = contentType,
                ReplyTo = replyTo,
                SequenceNumber = sequenceNumber,
                To = to,
                Label = label,
                CorrelationId = correlationId
            };

            log.LogInformation($"Got message via invocation {exCtx.InvocationId} on instance {Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID")} on process ID {Process.GetCurrentProcess().Id}: {Environment.NewLine} {data.ToPrettyString()}");
            
        }
    }

     
    public static class Dumper
    {
        public static string ToPrettyString(this object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented);
        }
    }
}
