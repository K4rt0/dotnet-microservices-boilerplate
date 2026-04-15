using MicroservicesBoilerplate.BuildingBlocks.Domain.Entities;

namespace MicroservicesBoilerplate.BuildingBlocks.Domain.Auditing;

public abstract class FullAuditedAggregateRoot<TId> : AuditedAggregateRoot<TId>, IFullAuditedObject
    where TId : notnull
{
    public virtual DateTime? DeletedAt { get; protected set; }
    public virtual Guid? DeletedBy { get; protected set; }
    public virtual bool IsDeleted { get; protected set; }

    protected FullAuditedAggregateRoot() { }
    protected FullAuditedAggregateRoot(TId id) : base(id) { }

    public virtual void SoftDelete(Guid? deletedBy = null)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }

    public virtual void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
    }
}