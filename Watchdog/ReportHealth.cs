using Watchdog.Queries;

namespace Watchdog
{
    public class ReportHealth 
    {
        private readonly InstanceIdentifier _statelessInstanceIdentifier;

        public ReportHealth(InstanceIdentifier statelessInstanceIdentifier)
        {
            _statelessInstanceIdentifier = statelessInstanceIdentifier;
        }

        public void Report()
        {

        }
    }
}
