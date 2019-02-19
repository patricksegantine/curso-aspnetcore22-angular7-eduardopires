using Eventos.IO.Domain.Core.Events;
using System;

namespace Eventos.IO.Domain.Core.Notifications
{
    public class DomainNotification : Event
    {
        public Guid DomainNotificationId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public int Version { get; set; }

        public DomainNotification(string key, string value)
        {
            DomainNotificationId = Guid.NewGuid();
            Key = key;
            Value = value;
            Version = 1;
        }
    }
}
