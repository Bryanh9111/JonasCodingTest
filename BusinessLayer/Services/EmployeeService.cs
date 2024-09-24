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
    /// Employee Service Layer
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="employeeRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public EmployeeService(
            IEmployeeRepository employeeRepository,
            ICompanyRepository companyRepository,
            IMapper mapper,
            ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get All Employees
        /// </summary>
        /// <returns>Returns a list of Employees</returns>
        public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeesAsync()
        {
            try
            {
                var employees = await _employeeRepository.GetAllAsync();
                var employeeInfos = _mapper.Map<IEnumerable<EmployeeInfo>>(employees);
                foreach (var employeeInfo in employeeInfos)
                {
                    var company = await _companyRepository.GetByCodeAsync(employeeInfo.CompanyCode);
                    if (company != null)
                        employeeInfo.CompanyName = company.CompanyName;
                }
                return employeeInfos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllEmployeesAsync.");
                throw;
            }
        }

        /// <summary>
        /// Get Employee By Code
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns>Returns an employee by employeeCode</returns>
        public async Task<EmployeeInfo> GetEmployeeByCodeAsync(string employeeCode)
        {
            if (string.IsNullOrEmpty(employeeCode))
                throw new ArgumentNullException(nameof(employeeCode));

            try
            {
                var employee = await _employeeRepository.GetByCodeAsync(employeeCode);
                if (employee == null)
                {
                    _logger.LogInformation("Employee not found. employeeCode - {employeeCode}");
                    return null;
                }
                var company = await _companyRepository.GetByCodeAsync(employee.CompanyCode);
                var employeeInfo = _mapper.Map<EmployeeInfo>(employee);
                employeeInfo.CompanyName = company?.CompanyName;
                return employeeInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in GetEmployeeByCodeAsync. employeeCode - {employeeCode}");
                throw;
            }
        }

        /// <summary>
        /// Create employee through EmployeeInfo model
        /// </summary>
        /// <param name="employeeToCreate"></param>
        /// <returns>Returns the created employee</returns>
        public async Task<EmployeeInfo> CreateEmployeeAsync(EmployeeInfo employeeToCreate)
        {
            if (employeeToCreate == null)
                throw new ArgumentNullException(nameof(employeeToCreate));

            try
            {
                var employeeEntityToCreate = _mapper.Map<Employee>(employeeToCreate);
                var employeeCreated = await _employeeRepository.AddAsync(employeeEntityToCreate);
                if (employeeCreated == null)
                {
                    _logger.LogInformation("Employee not created");
                    return null;
                }

                return await GetEmployeeByCodeAsync(employeeCreated.EmployeeCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in CreateEmployeeAsync.");
                throw;
            }
        }

        /// <summary>
        /// Update employee by employeeCode
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <param name="employeeToUpdate"></param>
        /// <returns>Returns the updated employee</returns>
        public async Task<EmployeeInfo> UpdateEmployeeAsync(string employeeCode, EmployeeInfo employeeToUpdate)
        {
            if (string.IsNullOrEmpty(employeeCode))
                throw new ArgumentNullException(nameof(employeeCode));
            if (employeeToUpdate == null)
                throw new ArgumentNullException(nameof(employeeToUpdate));

            try
            {
                var employeeEntityToUpdate = _mapper.Map<Employee>(employeeToUpdate);
                var employeeUpdated = await _employeeRepository.UpdateAsync(employeeCode, employeeEntityToUpdate);
                if (employeeUpdated == null)
                {
                    _logger.LogInformation("Employee not updated");
                    return null;
                }

                return await GetEmployeeByCodeAsync(employeeCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in UpdateEmployeeAsync. employeeCode - {employeeCode}");
                throw;
            }
        }

        /// <summary>
        /// Delete an employee by employeeCode
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns></returns>
        public async Task DeleteEmployeeAsync(string employeeCode)
        {
            if (string.IsNullOrEmpty(employeeCode))
                throw new ArgumentNullException(nameof(employeeCode));

            try
            {
                var deleted = await _employeeRepository.DeleteAsync(employeeCode);

                if (!deleted)
                    _logger.LogInformation($"Deletion failed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in DeleteEmployeeAsync. employeeCode - {employeeCode}");
                throw;
            }
        }
    }
}