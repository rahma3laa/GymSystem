using AutoMapper;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Session, SessionViewModel>()
                        .ForMember(dest => dest.CategoryName, Options => Options.MapFrom(scr => scr.SessionCategory.CategoryName))
                        .ForMember(dest => dest.TrainerName, Options => Options.MapFrom(scr => scr.SessionTrainer.Name))
                        .ForMember(dest => dest.AvailableStoles, Options => Options.Ignore());

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Session, UpdateSessionViewModel>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }

        
    }
}
        