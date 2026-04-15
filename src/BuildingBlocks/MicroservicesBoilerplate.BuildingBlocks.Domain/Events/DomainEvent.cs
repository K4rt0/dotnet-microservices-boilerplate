namespace MicroservicesBoilerplate.BuildingBlocks.Domain.Events;

public class DomainEvent
{
    public object EventData { get; }

    public long EventOrder { get; }

    public DomainEvent(object eventData, long eventOrder)
    {
        EventData = eventData;
        EventOrder = eventOrder;
    }
}