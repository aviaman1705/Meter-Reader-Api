using AutoMapper;
using MeterReaderAPI.DTO;
using MeterReaderAPI.DTO.Notebook;
using MeterReaderAPI.DTO.Track;
using MeterReaderAPI.DTO.User;
using MeterReaderAPI.Entities;
using Microsoft.AspNetCore.Identity;

namespace MeterReaderAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //track
            CreateMap<Track, TrackGridItem>().ForMember(x => x.NotebookNumber, dto => dto.MapFrom(prop => prop.Notebook.Number));
            CreateMap<TrackEditDTO, Track>();
            CreateMap<Track, TrackEditDTO>();
            CreateMap<Track, TrackDTO>().ReverseMap();
            CreateMap<Track, TrackCreationDTO>().ReverseMap();
            CreateMap<Track, SearchDTO>()
                .ForMember(x => x.Title, dto => dto.MapFrom(prop => prop.Desc))
                .ForMember(x => x.Link, dto => dto.MapFrom(prop => $"/tracks/edit/{prop.Id}"));

            //notebook
            CreateMap<Notebook, NotebookDTO>().ReverseMap();
            CreateMap<Notebook, AddNotebookDTO>().ReverseMap();

            //search
            CreateMap<SearchResult, SearchResultDTO>().ReverseMap();

            //user
            CreateMap<ApplicationUser, UserDetailsDTO>()
               .ForMember(x => x.Phone, dto => dto.MapFrom(prop => prop.PhoneNumber)).ReverseMap();
            CreateMap<RegisterDTO, UserCredentials>().ReverseMap();
            CreateMap<ApplicationUser, LoginDTO>()
                .ForMember(x => x.Email, dto => dto.MapFrom(prop => prop.Email))
                .ForMember(x => x.UserName, dto => dto.MapFrom(prop => prop.UserName));            

            //Dashboard
            CreateMap<Dashboard, DashboardDTO>().ReverseMap();
            CreateMap<DashboardSummary, DashboardSummaryDTO>().ReverseMap();
            CreateMap<MonthlyData, MonthlyDataDTO>().ReverseMap();
            CreateMap<PopularNotebook, PopularNotebookDTO>().ReverseMap();

        }
    }
}
