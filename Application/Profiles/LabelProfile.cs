using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;
public class LabelProfile : Profile
{
    public LabelProfile()
    {
        CreateMap<CreateLabelRequest, Label>();

        CreateMap<UpdateLabelRequest, Label>();

        CreateMap<Label, LabelResponse>();
    }
}
