namespace MicroservicesBoilerplate.BuildingBlocks.Domain.Entities;

public abstract class Entity<TId>
    where TId : notnull
{
    public virtual TId Id { get; protected set; } = default!;

    public Entity() { }

    public Entity(TId id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    public override bool Equals(object? obj)
        => obj is Entity<TId> other && EqualityComparer<TId>.Default.Equals(Id, other.Id);

    public override int GetHashCode()
        => Id is null ? 0 : EqualityComparer<TId>.Default.GetHashCode(Id);
}