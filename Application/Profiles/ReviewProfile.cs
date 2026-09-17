using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;
public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<CreateReviewRequest, Review>();
        CreateMap<UpdateReviewRequest, Review>();
        CreateMap<Review, ReviewResponse>();
    }
}
