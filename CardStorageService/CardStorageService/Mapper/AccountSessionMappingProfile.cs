using AutoMapper;
using CardStorageService.Data.Models;
using CardStorageService.Models.DTO;
using CardStorageService.Models.Requests;

namespace CardStorageService.Mapper
{
    public class AccountSessionMappingProfile : Profile
    {
        public AccountSessionMappingProfile()
        {
            CreateMap<AccountSession, AccountSessionDTO>().ReverseMap();
        }
    }
}
