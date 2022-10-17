using AutoMapper;
using Restaurant.Areas.Admin.ViewModels;
using Restaurant.Models;

namespace Restaurant.Areas.Admin.Mappings
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<Reservation, ReservationViewModel>();
            CreateMap<ReservationViewModel, Reservation>();
        }
    }
}
