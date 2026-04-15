namespace MicroservicesBoilerplate.BuildingBlocks.Domain.Entities;

public interface ISoftDelete
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    Guid? DeletedBy { get; }

    void SoftDelete(Guid? deletedBy = null);
    void Restore();
}
