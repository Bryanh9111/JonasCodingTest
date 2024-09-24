using DataAccessLayer.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Model.Interfaces
{
    /// <summary>
    /// Employee Repository Layer Contracts
    /// </summary>
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>Return a collection of Employee Entity</returns>
        Task<IEnumerable<Employee>> GetAllAsync();

        /// <summary>
        /// Get Employee Entity by employeeCode
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns>Return an employee</returns>
        Task<Employee> GetByCodeAsync(string employeeCode);

        /// <summary>
        /// Add an Employee Entity
        /// </summary>
        /// <param name="employeeToCreate"></param>
        /// <returns>Return the employee if successfully added</returns>
        Task<Employee> AddAsync(Employee employeeToCreate);

        /// <summary>
        /// Update an Employee Entity
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="employeeToUpdate"></param>
        /// <returns>Returns the employee if successfully updated</returns>
        Task<Employee> UpdateAsync(string employeeCode, Employee employeeToUpdate);

        /// <summary>
        /// Delete an Employee Entity
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns>Return true if successfully deleted, false if failed</returns>
        Task<bool> DeleteAsync(string employeeCode);
    }
}