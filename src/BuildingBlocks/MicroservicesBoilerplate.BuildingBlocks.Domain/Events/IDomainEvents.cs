namespace MicroservicesBoilerplate.BuildingBlocks.Domain.Events;

public interface IDomainEvents
{
    IEnumerable<DomainEvent> GetLocalEvents();
    IEnumerable<DomainEvent> GetDistributedEvents();
    void AddLocalEvent(object eventData);
    void AddDistributedEvent(object eventData);
    void ClearLocalEvents();
    void ClearDistributedEvents();
}