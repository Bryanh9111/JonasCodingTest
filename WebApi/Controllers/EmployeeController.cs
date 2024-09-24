using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebApi.Models;

namespace WebApi.Controllers
{
    /// <summary>
    /// Employee Controller Layer
    /// </summary>
    [RoutePrefix("api/employee")]
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET api/<controller>
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in GetAllAsync.");
                throw;
            }
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("{employeeCode}")]
        public async Task<EmployeeDto> GetAsync(string employeeCode)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByCodeAsync(employeeCode);
                return _mapper.Map<EmployeeDto>(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in GetAsync. employeeCode - {employeeCode}");
                throw;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Route("")]
        public async Task<EmployeeDto> PostAsync([FromBody] string value)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                var employeeDto = serializer.Deserialize<EmployeeDto>(value);//JsonConvert.DeserializeObject<EmployeeDto>(value);
                var employeeInfo = _mapper.Map<EmployeeInfo>(employeeDto);
                var employeeCreated = await _employeeService.CreateEmployeeAsync(employeeInfo);
                return _mapper.Map<EmployeeDto>(employeeCreated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in PostAsync. value - {value}");
                throw;
            }
        }

        // PUT api/<controller>/5
        [HttpPut]
        [Route("{code}")]
        public async Task<EmployeeDto> PutAsync(string code, [FromBody] string value)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                var employeeDto = serializer.Deserialize<EmployeeDto>(value);//JsonConvert.DeserializeObject<EmployeeDto>(value);
                var employeeInfo = _mapper.Map<EmployeeInfo>(employeeDto);
                var employeeUpdated = await _employeeService.UpdateEmployeeAsync(code, employeeInfo);
                return _mapper.Map<EmployeeDto>(employeeUpdated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in PutAsync. code - {code}, value - {value}");
                throw;
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        [Route("{code}")]
        public async Task DeleteAsync(string code)
        {
            try
            {
                await _employeeService.DeleteEmployeeAsync(code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in DeleDeleteAsyncte. code - {code}");
                throw;
            }
        }
    }
}
