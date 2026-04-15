using MicroservicesBoilerplate.BuildingBlocks.Domain.Entities;

namespace MicroservicesBoilerplate.BuildingBlocks.Domain.Auditing;

public abstract class AuditedEntity<TId> : Entity<TId>, IAuditedObject
    where TId : notnull
{
    public virtual DateTime CreatedAt { get; protected set; }
    public virtual Guid? CreatedBy { get; protected set; }
    public virtual DateTime? UpdatedAt { get; protected set; }
    public virtual Guid? UpdatedBy { get; protected set; }

    protected AuditedEntity() { }
    protected AuditedEntity(TId id) : base(id) { }
}