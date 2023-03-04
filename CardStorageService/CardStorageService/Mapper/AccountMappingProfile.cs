using AutoMapper;
using CardStorageService.Data.Models;
using CardStorageService.Models.DTO;
using CardStorageService.Models.Requests;

namespace CardStorageService.Mapper
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<Account, AccountDTO>().ReverseMap();
        }
    }
}
