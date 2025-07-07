using System;
using AutoMapper;
using TKApp.Business.DTOs;
using TKApp.Entities.Models;

namespace TKApp.Business.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User mappings
            CreateMap<User, UserResponse>();
            CreateMap<UserRequest, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.TenantId, opt => opt.Ignore())
                .ForMember(dest => dest.LastLoginDate, opt => opt.Ignore())
                .ForMember(dest => dest.FailedLoginAttempts, opt => opt.Ignore())
                .ForMember(dest => dest.IsLocked, opt => opt.Ignore())
                .ForMember(dest => dest.LockedUntil, opt => opt.Ignore())
                .ForMember(dest => dest.Vehicles, opt => opt.Ignore())
                .ForMember(dest => dest.Claims, opt => opt.Ignore());

            // Vehicle mappings
            CreateMap<Vehicle, VehicleDto>();
            CreateMap<VehicleRequest, Vehicle>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.TenantId, opt => opt.Ignore())
                .ForMember(dest => dest.LastLocationUpdate, opt => opt.Ignore())
                .ForMember(dest => dest.LastLatitude, opt => opt.Ignore())
                .ForMember(dest => dest.LastLongitude, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // Tenant mappings
            CreateMap<Tenant, TenantDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<TenantRequest, Tenant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // Location mappings
            CreateMap<LocationHistory, LocationHistoryDto>();
            CreateMap<LocationHistory, LocationDto>()
                .ForMember(dest => dest.VehicleId, opt => opt.MapFrom(src => src.VehicleId));
        }
    }
}
