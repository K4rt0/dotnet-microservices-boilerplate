using MicroservicesBoilerplate.BuildingBlocks.Domain.Entities;

namespace MicroservicesBoilerplate.BuildingBlocks.Domain.Auditing;

public interface IFullAuditedObject : IAuditedObject, ISoftDelete
{
}