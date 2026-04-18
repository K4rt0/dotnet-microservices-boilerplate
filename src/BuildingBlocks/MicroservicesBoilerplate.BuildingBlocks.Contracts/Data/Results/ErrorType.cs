namespace MicroservicesBoilerplate.BuildingBlocks.Contracts.Data.Results;

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
