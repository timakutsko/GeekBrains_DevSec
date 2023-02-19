using AutoMapper;
using CardStorageService.Data.Models;
using CardStorageService.Models.DTO;
using CardStorageService.Models.Requests;

namespace CardStorageService.Mapper
{
    public class CardMappingProfile : Profile
    {
        public CardMappingProfile()
        {
            CreateMap<Card, CardDTO>().ReverseMap();
            CreateMap<Card, CreateCardRequest>().ReverseMap();
        }
    }
}
