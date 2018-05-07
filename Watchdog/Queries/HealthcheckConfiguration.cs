using System;

namespace Watchdog.Queries
{
    [Serializable()]
    [System.Xml.Serialization.XmlRoot("Watchdog", Namespace = "http://schemas.microsoft.com/2015/03/fabact-no-schema")]
    public class HealthcheckConfiguration
    {
        public string Healthcheck { get; set; }
    }
}
