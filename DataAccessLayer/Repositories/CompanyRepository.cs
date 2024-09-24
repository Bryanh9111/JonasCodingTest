using AutoMapper;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Company Repository Layer
    /// </summary>
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDbWrapper<Company> _companyDbWrapper;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyRepository> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="companyDbWrapper"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public CompanyRepository(IDbWrapper<Company> companyDbWrapper, IMapper mapper, ILogger<CompanyRepository> logger)
        {
            _companyDbWrapper = companyDbWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all companies
        /// </summary>
        /// <returns>Return a collection of Company Entity</returns>
        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            try
            {
                return await _companyDbWrapper.FindAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllAsync.");
                throw;
            }
        }

        /// <summary>
        /// Get Company Entity by companyCode
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns>Return a company</returns>
        public async Task<Company> GetByCodeAsync(string companyCode)
        {
            if (string.IsNullOrEmpty(companyCode))
                throw new ArgumentNullException(nameof(companyCode));

            try
            {
                var companies = await _companyDbWrapper.FindAsync(t => t.CompanyCode.Equals(companyCode));
                return companies?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in GetByCodeAsync. companyCode - {companyCode}");
                throw;
            }
        }

        /// <summary>
        /// Add a Company Entity
        /// </summary>
        /// <param name="companyToCreate"></param>
        /// <returns>Return the company if successfully added</returns>
        public async Task<Company> AddAsync(Company companyToCreate)
        {
            if (companyToCreate == null)
                throw new ArgumentNullException(nameof(companyToCreate));

            try
            {
                var companies = await _companyDbWrapper.FindAsync(t =>
                    t.SiteId.Equals(companyToCreate.SiteId) && t.CompanyCode.Equals(companyToCreate.CompanyCode));

                var existingCompany = companies?.FirstOrDefault();
                if (existingCompany != null)
                {
                    _logger.LogInformation($"Company exists with siteId: {companyToCreate.SiteId} and companyCode: {companyToCreate.CompanyCode}");
                    return null;
                }

                if (!await _companyDbWrapper.InsertAsync(companyToCreate))
                {
                    _logger.LogInformation($"Insertion failed.");
                    return null;
                }

                return companyToCreate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in AddAsync.");
                throw;
            }
        }

        /// <summary>
        /// Update a Company Entity
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="companyToUpdate"></param>
        /// <returns>Returns the company if successfully updated</returns>
        public async Task<Company> UpdateAsync(string siteId, Company companyToUpdate)
        {
            if (string.IsNullOrEmpty(siteId))
                throw new ArgumentNullException(nameof(siteId));
            if (companyToUpdate == null)
                throw new ArgumentNullException(nameof(companyToUpdate));

            try
            {
                var companies = await _companyDbWrapper.FindAsync(t =>
                    t.SiteId.Equals(siteId) && t.CompanyCode.Equals(companyToUpdate.CompanyCode));

                var existingCompanyToUpdate = companies?.FirstOrDefault();
                if (existingCompanyToUpdate == null)
                {
                    _logger.LogInformation($"Company not found with siteId: {siteId}, companyCode - {companyToUpdate.CompanyCode}");
                    return null;
                }

                _mapper.Map(companyToUpdate, existingCompanyToUpdate);

                if (!await _companyDbWrapper.UpdateAsync(existingCompanyToUpdate))
                {
                    _logger.LogInformation($"Update failed.");
                    return null;
                }

                return existingCompanyToUpdate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in UpdateAsync. siteId - {siteId}");
                throw;
            }
        }

        /// <summary>
        /// Delete a Company Entity
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns>Return true if successfully deleted, false if failed</returns>
        public async Task<bool> DeleteAsync(string siteId)
        {
            if (string.IsNullOrEmpty(siteId))
                throw new ArgumentNullException(nameof(siteId));

            try
            {
                var deleted = await _companyDbWrapper.DeleteAsync(t => t.SiteId.Equals(siteId));
                if (!deleted)
                    _logger.LogInformation($"Deletion failed. siteId - {siteId}");

                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in DeleteAsync. siteId - {siteId}");
                throw;
            }
        }

        /// <summary>
        /// Original method provided to do both insertion and update together in one place
        /// </summary>
        /// <param name="company"></param>
        /// <returns>Return true if the operation is a success</returns>
        public async Task<bool> SaveCompanyAsync(Company company)
        {
            try
            {
                var itemRepoList = await _companyDbWrapper.FindAsync(t =>
                    t.SiteId.Equals(company.SiteId) && t.CompanyCode.Equals(company.CompanyCode));

                var itemRepo = itemRepoList?.FirstOrDefault();

                if (itemRepo != null)
                {
                    itemRepo.CompanyName = company.CompanyName;
                    itemRepo.AddressLine1 = company.AddressLine1;
                    itemRepo.AddressLine2 = company.AddressLine2;
                    itemRepo.AddressLine3 = company.AddressLine3;
                    itemRepo.Country = company.Country;
                    itemRepo.EquipmentCompanyCode = company.EquipmentCompanyCode;
                    itemRepo.FaxNumber = company.FaxNumber;
                    itemRepo.PhoneNumber = company.PhoneNumber;
                    itemRepo.PostalZipCode = company.PostalZipCode;
                    itemRepo.LastModified = company.LastModified;
                    return await _companyDbWrapper.UpdateAsync(itemRepo);
                }

                return await _companyDbWrapper.InsertAsync(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in SaveCompanyAsync.");
                throw;
            }
        }
    }
}
