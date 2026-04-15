namespace MicroservicesBoilerplate.BuildingBlocks.Domain.Results;

public enum ErrorType
{
    None,
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden,
    Unexpected
}
