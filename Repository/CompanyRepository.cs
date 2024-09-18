using Contracts;
using Entities.Models;

namespace Repository;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public IEnumerable<Company> GetAllCompanies(bool trackChanges)
        => FindAll(trackChanges).OrderBy(c => c.Name).ToList();

    public Company? GetCompany(Guid companyId, bool trackChange)
        => FindByCondition(c => c.Id.Equals(companyId), trackChange)
            .SingleOrDefault();

    public void CreateCompany(Company company) => Create(company);


    public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
        FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToList();

    public void DeleteCompany(Company company) => Delete(company);
}