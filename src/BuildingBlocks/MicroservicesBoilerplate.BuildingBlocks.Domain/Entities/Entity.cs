namespace MicroservicesBoilerplate.BuildingBlocks.Domain.Entities;

public abstract class Entity<TId>
    where TId : notnull
{
    public virtual TId Id { get; protected set; } = default!;

    protected Entity() { }

    protected Entity(TId id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (EqualityComparer<TId>.Default.Equals(Id, default!) ||
            EqualityComparer<TId>.Default.Equals(other.Id, default!))
            return false;
        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode()
        => EqualityComparer<TId>.Default.Equals(Id, default!) ? base.GetHashCode() : Id.GetHashCode();
}