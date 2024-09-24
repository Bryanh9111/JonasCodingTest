using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    /// <summary>
    /// Company Service Layer
    /// </summary>
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyService> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="companyRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public CompanyService(ICompanyRepository companyRepository, IMapper mapper, ILogger<CompanyService> logger)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get All Companies
        /// </summary>
        /// <returns>Returns a list of Companies</returns>
        public async Task<IEnumerable<CompanyInfo>> GetAllCompaniesAsync()
        {
            try
            {
                var companies = await _companyRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CompanyInfo>>(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllCompaniesAsync.");
                throw;
            }
        }

        /// <summary>
        /// Get Company By Code
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns>Returns a company by companyCode</returns>
        public async Task<CompanyInfo> GetCompanyByCodeAsync(string companyCode)
        {
            if (string.IsNullOrEmpty(companyCode))
                throw new ArgumentNullException(nameof(companyCode));

            try
            {
                var company = await _companyRepository.GetByCodeAsync(companyCode);
                return _mapper.Map<CompanyInfo>(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in GetCompanyByCodeAsync. companyCode - {companyCode}");
                throw;
            }
        }

        /// <summary>
        /// Create Company through CompanyInfo model
        /// </summary>
        /// <param name="companyToCreate"></param>
        /// <returns>Returns the created Company</returns>
        public async Task<CompanyInfo> CreateCompanyAsync(CompanyInfo companyToCreate)
        {
            if (companyToCreate == null)
                throw new ArgumentNullException(nameof(companyToCreate));

            try
            {
                var companyEntityToCreate = _mapper.Map<Company>(companyToCreate);
                var companyCreated = await _companyRepository.AddAsync(companyEntityToCreate);
                if (companyCreated == null)
                {
                    _logger.LogInformation("Company not created");
                    return null;
                }

                return _mapper.Map<CompanyInfo>(companyCreated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in CreateCompanyAsync.");
                throw;
            }
        }

        /// <summary>
        /// Update Company by siteId
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="companyToUpdate"></param>
        /// <returns>Returns the updated Company</returns>
        public async Task<CompanyInfo> UpdateCompanyAsync(string siteId, CompanyInfo companyToUpdate)
        {
            if (string.IsNullOrEmpty(siteId))
                throw new ArgumentNullException(nameof(siteId));
            if (companyToUpdate == null)
                throw new ArgumentNullException(nameof(companyToUpdate));

            try
            {
                var companyEntityToUpdate = _mapper.Map<Company>(companyToUpdate);
                var companyUpdated = await _companyRepository.UpdateAsync(siteId, companyEntityToUpdate);
                if (companyUpdated == null)
                {
                    _logger.LogInformation("Company not updated");
                    return null;
                }

                return _mapper.Map<CompanyInfo>(companyUpdated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in UpdateCompanyAsync. siteId - {siteId}");
                throw;
            }
        }

        /// <summary>
        /// Delete a company by siteId
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task DeleteCompanyAsync(string siteId)
        {
            if (string.IsNullOrEmpty(siteId))
                throw new ArgumentNullException(nameof(siteId));

            try
            {
                var deleted = await _companyRepository.DeleteAsync(siteId);

                if (!deleted)
                    _logger.LogInformation($"Deletion failed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in DeleteCompanyAsync. siteId - {siteId}");
                throw;
            }
        }
    }
}
