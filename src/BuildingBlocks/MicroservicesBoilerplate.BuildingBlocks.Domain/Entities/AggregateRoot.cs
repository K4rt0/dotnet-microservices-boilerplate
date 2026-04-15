using MicroservicesBoilerplate.BuildingBlocks.Domain.Events;

namespace MicroservicesBoilerplate.BuildingBlocks.Domain.Entities;

public abstract class AggregateRoot<TId> : Entity<TId>, IHasConcurrencyStamp, IDomainEvents
    where TId : notnull
{
    public virtual string ConcurrencyStamp { get; set; }
    private ICollection<DomainEvent>? _distributedEvents;
    private ICollection<DomainEvent>? _localEvents;

    public AggregateRoot()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    protected AggregateRoot(TId id) : base(id)
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    public IEnumerable<DomainEvent> GetLocalEvents() => _localEvents ?? Array.Empty<DomainEvent>();
    public IEnumerable<DomainEvent> GetDistributedEvents() => _distributedEvents ?? Array.Empty<DomainEvent>();
    public void ClearLocalEvents() => _localEvents?.Clear();
    public void ClearDistributedEvents() => _distributedEvents?.Clear();

    public void AddLocalEvent(object eventData)
    {
        _localEvents ??= new List<DomainEvent>();
        _localEvents.Add(new DomainEvent(eventData, EventOrderGenerator.GetNext()));
    }

    public void AddDistributedEvent(object eventData)
    {
        _distributedEvents ??= new List<DomainEvent>();
        _distributedEvents.Add(new DomainEvent(eventData, EventOrderGenerator.GetNext()));
    }
}