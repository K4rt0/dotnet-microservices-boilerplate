namespace MicroservicesBoilerplate.BuildingBlocks.Domain.Auditing;

public interface IAuditedObject
{
    DateTime CreatedAt { get; }
    Guid? CreatedBy { get; }
    DateTime? UpdatedAt { get; }
    Guid? UpdatedBy { get; }
}