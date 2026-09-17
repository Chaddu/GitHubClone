using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;
public class BranchProfile : Profile
{
    public BranchProfile()
    {
        CreateMap<CreateBranchRequest, Branch>();

        CreateMap<UpdateBranchRequest, Branch>();

        CreateMap<Branch, BranchResponse>();
    }
}
