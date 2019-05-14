using System;
using System.IO;
using log4net.Core;
using log4net.Layout;
using Microsoft.Azure.Cosmos.Table;

namespace log4net.Appender.Azure.Core
{
    internal sealed class AzureLayoutLoggingEventEntity : TableEntity
    {
        public AzureLayoutLoggingEventEntity(LoggingEvent e, PartitionKeyTypeEnum partitionKeyType, ILayout layout)
        {
            Level = e.Level.ToString();
            Message = e.RenderedMessage + Environment.NewLine + e.GetExceptionString();
            ThreadName = e.ThreadName;
            EventTimeStamp = e.TimeStamp;
            using (var w = new StringWriter())
            {
                layout.Format(w, e);
                Message = w.ToString();
            }

            PartitionKey = e.MakePartitionKey(partitionKeyType);
            RowKey = e.MakeRowKey();
        }

        public DateTime EventTimeStamp { get; set; }

        public string ThreadName { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }
    }
}
