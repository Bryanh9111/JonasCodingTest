﻿using AutoMapper;
using BusinessLayer.Model.Models;
using WebApi.Models;

namespace WebApi
{
    public class AppServicesProfile : Profile
    {
        public AppServicesProfile()
        {
            CreateMapper();
        }

        private void CreateMapper()
        {
            CreateMap<BaseInfo, BaseDto>().ReverseMap();

            CreateMap<CompanyInfo, CompanyDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.AddressLine1))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.AddressLine2))
                .ForMember(dest => dest.AddressLine3, opt => opt.MapFrom(src => src.AddressLine3))
                .ForMember(dest => dest.PostalZipCode, opt => opt.MapFrom(src => src.PostalZipCode))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.FaxNumber, opt => opt.MapFrom(src => src.FaxNumber))
                .ForMember(dest => dest.EquipmentCompanyCode, opt => opt.MapFrom(src => src.EquipmentCompanyCode))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.ArSubledgers, opt => opt.MapFrom(src => src.ArSubledgers))
                .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.LastModified))
                .ReverseMap();

            CreateMap<ArSubledgerInfo, ArSubledgerDto>()
                .ForMember(dest => dest.ArSubledgerCode, opt => opt.MapFrom(src => src.ArSubledgerCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.AddressLine1))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.AddressLine2))
                .ForMember(dest => dest.AddressLine3, opt => opt.MapFrom(src => src.AddressLine3))
                .ForMember(dest => dest.PostalZipCode, opt => opt.MapFrom(src => src.PostalZipCode))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.FaxNumber, opt => opt.MapFrom(src => src.FaxNumber))
                .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.LastModified))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                .ForMember(dest => dest.Inactive, opt => opt.MapFrom(src => src.Inactive))
                .ForMember(dest => dest.Excellent, opt => opt.MapFrom(src => src.Excellent))
                .ForMember(dest => dest.Good, opt => opt.MapFrom(src => src.Good))
                .ForMember(dest => dest.Fair, opt => opt.MapFrom(src => src.Fair))
                .ForMember(dest => dest.Poor, opt => opt.MapFrom(src => src.Poor))
                .ForMember(dest => dest.Condemned, opt => opt.MapFrom(src => src.Condemned))
                .ReverseMap();

            CreateMap<EmployeeInfo, EmployeeDto>()
                .ForMember(dest => dest.EmployeeCode, opt => opt.MapFrom(src => src.EmployeeCode))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.EmployeeName))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.OccupationName, opt => opt.MapFrom(src => src.Occupation))
                .ForMember(dest => dest.EmployeeStatus, opt => opt.MapFrom(src => src.EmployeeStatus))
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.LastModifiedDateTime, opt => opt.MapFrom(src => src.LastModified.ToString("yyyy-MM-dd HH:mm:ss")))
                .ReverseMap();
        }
    }
}