namespace FluentNHSampleApp.Domain
{
    public class EntityBase
    {
        public virtual long Id { get; set; }

        public virtual bool Equals(EntityBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (EntityBase)) return false;
            return Equals((EntityBase) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}