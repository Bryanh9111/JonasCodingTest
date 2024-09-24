using BusinessLayer.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Model.Interfaces
{
    /// <summary>
    /// Employee Service Layer Contracts
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Get All Employees
        /// </summary>
        /// <returns>Returns a list of Employees</returns>
        Task<IEnumerable<EmployeeInfo>> GetAllEmployeesAsync();

        /// <summary>
        /// Get Employee By Code
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns>Returns an employee by employeeCode</returns>
        Task<EmployeeInfo> GetEmployeeByCodeAsync(string employeeCode);

        /// <summary>
        /// Create employee through EmployeeInfo model
        /// </summary>
        /// <param name="employeeToCreate"></param>
        /// <returns>Returns the created employee</returns>
        Task<EmployeeInfo> CreateEmployeeAsync(EmployeeInfo employeeToCreate);

        /// <summary>
        /// Update employee by employeeCode
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <param name="employeeToUpdate"></param>
        /// <returns>Returns the updated employee</returns>
        Task<EmployeeInfo> UpdateEmployeeAsync(string employeeCode, EmployeeInfo employeeToUpdate);

        /// <summary>
        /// Delete an employee by employeeCode
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns></returns>
        Task DeleteEmployeeAsync(string employeeCode);
    }
}