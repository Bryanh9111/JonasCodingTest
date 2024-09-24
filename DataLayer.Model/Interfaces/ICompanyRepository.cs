using DataAccessLayer.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Model.Interfaces
{
    /// <summary>
    /// Company Repository Layer Contracts
    /// </summary>
    public interface ICompanyRepository
    {
        /// <summary>
        /// Get all companies
        /// </summary>
        /// <returns>Return a collection of Company Entity</returns>
        Task<IEnumerable<Company>> GetAllAsync();

        /// <summary>
        /// Get Company Entity by companyCode
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns>Return a company</returns>
        Task<Company> GetByCodeAsync(string companyCode);

        /// <summary>
        /// Add a Company Entity
        /// </summary>
        /// <param name="companyToCreate"></param>
        /// <returns>Return the company if successfully added</returns>
        Task<Company> AddAsync(Company companyToCreate);

        /// <summary>
        /// Update a Company Entity
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="companyToUpdate"></param>
        /// <returns>Returns the company if successfully updated</returns>
        Task<Company> UpdateAsync(string siteId, Company companyToUpdate);

        /// <summary>
        /// Delete a Company Entity
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns>Return true if successfully deleted, false if failed</returns>
        Task<bool> DeleteAsync(string siteId);

        /// <summary>
        /// Original method provided to do both insertion and update
        /// </summary>
        /// <param name="company"></param>
        /// <returns>Return true if the operation is a success</returns>
        Task<bool> SaveCompanyAsync(Company company);
    }
}
