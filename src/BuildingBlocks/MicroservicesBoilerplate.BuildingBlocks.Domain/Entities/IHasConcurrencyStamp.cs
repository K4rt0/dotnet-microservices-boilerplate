namespace MicroservicesBoilerplate.BuildingBlocks.Domain.Entities;

public interface IHasConcurrencyStamp
{
    string ConcurrencyStamp { get; set; }
}