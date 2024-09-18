namespace Entities.Exceptions;

public sealed class CompanyNotFoundException : NotFoundException
{
    public CompanyNotFoundException(Guid companyGuid) :
        base($"The company with id: {companyGuid} doesn't exist in the database.")
    {

    }
}