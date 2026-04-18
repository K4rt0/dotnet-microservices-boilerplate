namespace MicroservicesBoilerplate.BuildingBlocks.Domain.Entities;

public interface IEntity<TId>
    where TId : notnull
{
    TId Id { get; }
}