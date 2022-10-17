using AutoMapper;
using Restaurant.Areas.Admin.ViewModels;
using Restaurant.Models;

namespace Restaurant.Areas.Admin.Mappings
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewViewModel>();
            CreateMap<ReviewViewModel, Review>();
        }
    }
}
