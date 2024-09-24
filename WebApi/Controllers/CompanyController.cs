using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    /// <summary>
    /// Company Controller Layer
    /// </summary>
    [RoutePrefix("api/company")]
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyService companyService, IMapper mapper, ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET api/<controller>
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<CompanyDto>> GetAllAsync()
        {
            try
            {
                var companies = await _companyService.GetAllCompaniesAsync();
                return _mapper.Map<IEnumerable<CompanyDto>>(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in GetAllAsync.");
                throw;
            }
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("{companyCode}")]
        public async Task<CompanyDto> GetAsync(string companyCode)
        {
            try
            {
                var company = await _companyService.GetCompanyByCodeAsync(companyCode);
                return _mapper.Map<CompanyDto>(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in GetAsync. companyCode - {companyCode}");
                throw;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Route("")]
        public async Task<CompanyDto> PostAsync([FromBody] string value)
        {
            try
            {
                var companyDto = JsonConvert.DeserializeObject<CompanyDto>(value);
                var companyInfo = _mapper.Map<CompanyInfo>(companyDto);
                var companyCreated = await _companyService.CreateCompanyAsync(companyInfo);
                return _mapper.Map<CompanyDto>(companyCreated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in PostAsync. value - {value}");
                throw;
            }
        }

        // PUT api/<controller>/5
        [HttpPut]
        [Route("{id}")]
        public async Task<CompanyDto> PutAsync(int id, [FromBody] string value)
        {
            try
            {
                var companyDto = JsonConvert.DeserializeObject<CompanyDto>(value);
                var companyInfo = _mapper.Map<CompanyInfo>(companyDto);
                var companyUpdated = await _companyService.UpdateCompanyAsync(id.ToString(), companyInfo);
                return _mapper.Map<CompanyDto>(companyUpdated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in PutAsync. id - {id}, value - {value}");
                throw;
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(int id)
        {
            try
            {
                await _companyService.DeleteCompanyAsync(id.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred in DeleteAsync. id - {id}");
                throw;
            }
        }
    }
}