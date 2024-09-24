using BusinessLayer.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Model.Interfaces
{
    /// <summary>
    /// Company Service Layer Contracts
    /// </summary>
    public interface ICompanyService
    {
        /// <summary>
        /// Get All Companies
        /// </summary>
        /// <returns>Returns a list of Companies</returns>
        Task<IEnumerable<CompanyInfo>> GetAllCompaniesAsync();

        /// <summary>
        /// Get Company By Code
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns>Returns a company by companyCode</returns>
        Task<CompanyInfo> GetCompanyByCodeAsync(string companyCode);

        /// <summary>
        /// Create Company through CompanyInfo model
        /// </summary>
        /// <param name="companyToCreate"></param>
        /// <returns>Returns the created Company</returns>
        Task<CompanyInfo> CreateCompanyAsync(CompanyInfo companyToCreate);

        /// <summary>
        /// Update Company by siteId
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="companyToUpdate"></param>
        /// <returns>Returns the updated Company</returns>
        Task<CompanyInfo> UpdateCompanyAsync(string siteId, CompanyInfo companyToUpdate);

        /// <summary>
        /// Delete a company by siteId
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task DeleteCompanyAsync(string siteId);
    }
}
