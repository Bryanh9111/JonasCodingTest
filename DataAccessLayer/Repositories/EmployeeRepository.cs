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
    /// Employee Repository Layer
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbWrapper<Employee> _employeeDbWrapper;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeRepository> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="employeeDbWrapper"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public EmployeeRepository(IDbWrapper<Employee> employeeDbWrapper, IMapper mapper, ILogger<EmployeeRepository> logger)
        {
            _employeeDbWrapper = employeeDbWrapper;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>Return a collection of Employee Entity</returns>
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            try
            {
                return await _employeeDbWrapper.FindAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllAsync.");
                throw;
            }
        }

        /// <summary>
        /// Get Employee Entity by employeeCode
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns>Return an employee</returns>
        public async Task<Employee> GetByCodeAsync(string employeeCode)
        {
            if (string.IsNullOrEmpty(employeeCode))
                throw new ArgumentNullException(nameof(employeeCode));

            try
            {
                var employees = await _employeeDbWrapper.FindAsync(t => t.EmployeeCode.Equals(employeeCode));
                return employees?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in GetByCodeAsync. employeeCode - {employeeCode}");
                throw;
            }
        }

        /// <summary>
        /// Add an Employee Entity
        /// </summary>
        /// <param name="employeeToCreate"></param>
        /// <returns>Return the employee if successfully added</returns>
        public async Task<Employee> AddAsync(Employee employeeToCreate)
        {
            if (employeeToCreate == null)
                throw new ArgumentNullException(nameof(employeeToCreate));

            try
            {
                var employees = await _employeeDbWrapper.FindAsync(t => t.EmployeeCode.Equals(employeeToCreate.EmployeeCode));

                var existingEmployee = employees?.FirstOrDefault();
                if (existingEmployee != null)
                {
                    _logger.LogInformation($"Employee exists with employeeCode: {employeeToCreate.EmployeeCode}");
                    return null;
                }

                if (!await _employeeDbWrapper.InsertAsync(employeeToCreate))
                {
                    _logger.LogInformation($"Insertion failed.");
                    return null;
                }

                return employeeToCreate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in AddAsync.");
                throw;
            }
        }

        /// <summary>
        /// Update an Employee Entity
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="employeeToUpdate"></param>
        /// <returns>Returns the employee if successfully updated</returns>
        public async Task<Employee> UpdateAsync(string employeeCode, Employee employeeToUpdate)
        {
            if (string.IsNullOrEmpty(employeeCode))
                throw new ArgumentNullException(nameof(employeeCode));
            if (employeeToUpdate == null)
                throw new ArgumentNullException(nameof(employeeToUpdate));

            try
            {
                var employees = await _employeeDbWrapper.FindAsync(t => t.EmployeeCode.Equals(employeeCode));

                var existingEmployeeToUpdate = employees?.FirstOrDefault();
                if (existingEmployeeToUpdate == null)
                {
                    _logger.LogInformation($"Employee not found with employeeCode - {employeeCode}");
                    return null;
                }

                _mapper.Map(employeeToUpdate, existingEmployeeToUpdate);

                if (!await _employeeDbWrapper.UpdateAsync(existingEmployeeToUpdate))
                {
                    _logger.LogInformation($"Update failed.");
                    return null;
                }

                return existingEmployeeToUpdate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in UpdateAsync. employeeCode - {employeeCode}");
                throw;
            }
        }

        /// <summary>
        /// Delete an Employee Entity
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns>Return true if successfully deleted, false if failed</returns>
        public async Task<bool> DeleteAsync(string employeeCode)
        {
            if (string.IsNullOrEmpty(employeeCode))
                throw new ArgumentNullException(nameof(employeeCode));

            try
            {
                var deleted = await _employeeDbWrapper.DeleteAsync(t => t.EmployeeCode.Equals(employeeCode));
                if (!deleted)
                    _logger.LogInformation($"Deletion failed. employeeCode - {employeeCode}");

                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in DeleteAsync. employeeCode - {employeeCode}");
                throw;
            }
        }
    }
}