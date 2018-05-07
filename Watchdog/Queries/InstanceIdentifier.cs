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

        protected bool Equals(InstanceIdentifier other)
        {
            return PartitionId.Equals(other.PartitionId) && InstanceId == other.InstanceId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InstanceIdentifier) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (PartitionId.GetHashCode() * 397) ^ InstanceId.GetHashCode();
            }
        }
    }
}
