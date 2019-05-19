using System;

namespace GarbageCollector.Domain
{
    public class WasteCategory : IEquatable<WasteCategory>
    {
        public bool Equals(WasteCategory other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id) && string.Equals(Name, other.Name) && Material == other.Material;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WasteCategory) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Material;
                return hashCode;
            }
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
        public Material Material { get; set; }
    }
}