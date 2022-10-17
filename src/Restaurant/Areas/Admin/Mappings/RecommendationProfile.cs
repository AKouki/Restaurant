using AutoMapper;
using Restaurant.Areas.Admin.ViewModels;
using Restaurant.Models;

namespace Restaurant.Areas.Admin.Mappings
{
    public class RecommendationProfile : Profile
    {
        public RecommendationProfile()
        {
            CreateMap<Recommendation, RecommendationViewModel>();
            CreateMap<RecommendationViewModel, Recommendation>();

            CreateMap<Recommendation, AddRecommendationViewModel>();
            CreateMap<AddRecommendationViewModel, Recommendation>();
        }
    }
}
