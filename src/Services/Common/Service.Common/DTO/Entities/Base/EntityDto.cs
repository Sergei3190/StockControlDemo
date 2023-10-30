using Service.Common.Entities.Base.Interfaces;

namespace Service.Common.DTO.Entities.Base;

public abstract class EntityDto : IEntity, IEquatable<EntityDto>
{
	public Guid Id { get; set; }

	public bool Equals(EntityDto? other)
	{
		if (other is null) return false;
		if (ReferenceEquals(this, other)) return true;
		return Id == other.Id;
	}

	public override bool Equals(object? obj)
	{
		if (obj is null) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != GetType()) return false;
		return base.Equals((EntityDto)obj);
	}

	public override int GetHashCode() => Id.GetHashCode();

	public static bool operator ==(EntityDto? left, EntityDto? right) => Equals(left, right);
	public static bool operator !=(EntityDto? left, EntityDto? right) => !Equals(left, right);
}