using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;
public class ActivityProfile : Profile
{
    public ActivityProfile()
    {
        CreateMap<Activity, ActivityResponse>();
    }
}
