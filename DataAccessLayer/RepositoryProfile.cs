using AutoMapper;
using DataAccessLayer.Model.Models;
using System;

namespace DataAccessLayer
{
    public class RepositoryProfile : Profile
    {
        public RepositoryProfile()
        {
            CreateMapper();
        }

        private void CreateMapper()
        {
            CreateMap<Company, Company>()
               .ForMember(dest => dest.SiteId, opt => opt.Ignore())
               .ForMember(dest => dest.CompanyCode, opt => opt.Ignore())
               .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Employee, Employee>()
               .ForMember(dest => dest.SiteId, opt => opt.Ignore())
               .ForMember(dest => dest.CompanyCode, opt => opt.Ignore())
               .ForMember(dest => dest.EmployeeCode, opt => opt.Ignore())
               .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}