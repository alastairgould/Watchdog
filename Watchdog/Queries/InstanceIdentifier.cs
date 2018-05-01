using System;

namespace Watchdog.Queries
{
    public class InstanceIdentifier
    {
        public Guid PartitionId { get; }
        public long InstanceId { get; }

        public InstanceIdentifier(Guid partitionId, long instanceId)
        {
            PartitionId = partitionId;
            InstanceId = instanceId;
        }
    }
}
