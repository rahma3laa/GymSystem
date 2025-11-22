using AutoMapper;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
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
            MapSessions();

            MapMember();

            MapTrainer();

            MapPlan();
        }

        private void MapSessions()
        {
            CreateMap<Session, SessionViewModel>()
                       .ForMember(dest => dest.CategoryName, Options => Options.MapFrom(scr => scr.Category.CategoryName))
                       .ForMember(dest => dest.TrainerName, Options => Options.MapFrom(scr => scr.Trainer.Name))
                       .ForMember(dest => dest.AvailableStoles, Options => Options.Ignore());

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Session, UpdateSessionViewModel>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
        }

        private void MapMember()
        {
            //CreateMap<CreateMemberViewModel, Member>()
            //    .ForMember(dest => dest.Address, opt => opt.MapFrom(scr => new Address()
            //    {
            //        BuildingNumber = scr.BuildingNumber,
            //        Street = scr.Street,
            //        City = scr.City,

            //    }));
            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(scr => scr))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(scr => scr.HealthRecordViewModel));

            CreateMap<CreateMemberViewModel, Address>()
             .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(scr => scr.BuildingNumber))
             .ForMember(dest => dest.City, opt => opt.MapFrom(scr => scr.City))
             .ForMember(dest => dest.Street, opt => opt.MapFrom(scr => scr.Street));

            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();

            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(scr => scr.Gender.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(scr => scr.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));


            CreateMap<Member, MemberToUpdateViewModel>()
                  .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(scr => scr.Address.BuildingNumber))
                  .ForMember(dest => dest.Street, opt => opt.MapFrom(scr => scr.Address.Street))
                  .ForMember(dest => dest.City, opt => opt.MapFrom(scr => scr.Address.City));

            CreateMap<MemberToUpdateViewModel, Member>()
                  .ForMember(dest => dest.Name, opt => opt.Ignore())
                    .ForMember(dest => dest.Photo, opt => opt.Ignore())
                    .AfterMap((src, dest) =>
                    {
                        dest.Address.BuildingNumber = src.BuildingNumber;
                        dest.Address.Street = src.Street;
                        dest.Address.City = src.City;
                        dest.UpdatedAt = DateTime.Now;
                    });
                 
        }


        private void MapTrainer()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    Street = src.Street,
                    City = src.City,
                    BuildingNumber = src.BuildingNumber
                }));

            CreateMap<Trainer, TrainerViewModel>();
            CreateMap<Trainer , UpdatedTrainerViewModel>()
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber));

            CreateMap<UpdatedTrainerViewModel, Trainer>()
                .ForMember(dest=>dest.Name , opt=>opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.City = src.City;
                    dest.Address.Street = src.Street;
                    dest.UpdatedAt = DateTime.Now;
                });
        }

        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, PlanViewModel>().ForMember(dest => dest.Name, opt => opt.MapFrom(scr => scr.Name));
            CreateMap<UpdatePlanViewModel, Plan>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(scr => DateTime.Now));
        }   
    }
}
        