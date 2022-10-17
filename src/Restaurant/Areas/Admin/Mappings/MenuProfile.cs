using AutoMapper;
using Restaurant.Areas.Admin.ViewModels;
using Restaurant.Models;

namespace Restaurant.Areas.Admin.Mappings
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<Menu, MenuViewModel>();
            CreateMap<MenuViewModel, Menu>();
            CreateMap<MenuItem, MenuItemViewModel>();
            CreateMap<MenuItemViewModel, MenuItem>();
        }
    }
}
