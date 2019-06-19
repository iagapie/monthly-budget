using System;

namespace IA.Finance.Domain.SeedWork
{
    public abstract class Entity
    {
        private int? _requestedHashCode;
        
        private long _id;

        private DateTimeOffset _createdAt = DateTimeOffset.UtcNow;

        private DateTimeOffset? _updatedAt;
        
        public virtual long Id 
        {
            get => _id;
            protected set => _id = value;
        }

        public DateTimeOffset CreatedAt
        {
            get => _createdAt;
            protected set => _createdAt = value;
        }

        public DateTimeOffset? UpdatedAt
        {
            get => _updatedAt;
            protected set => _updatedAt = value;
        }

        public void SetUpdatedAt() => UpdatedAt = DateTimeOffset.UtcNow;

        public bool IsTransient() => Id == default(long);

        public override bool Equals(object obj)
        {
            if (!(obj is Entity))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            var item = (Entity)obj;

            if (item.IsTransient() || IsTransient())
                return false;
            
            return item.Id == Id;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return _requestedHashCode.Value;
            }
            
            return base.GetHashCode();

        }
        public static bool operator ==(Entity left, Entity right)
        {
            if (Object.Equals(left, null))
                return Object.Equals(right, null);
            
            return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right) => !(left == right);
    }
}