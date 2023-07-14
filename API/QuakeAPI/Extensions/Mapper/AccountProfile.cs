using AutoMapper;
using QuakeAPI.Data.Models;
using QuakeAPI.DTO;

namespace QuakeAPI.Extensions.Mapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Account, AccountDto>();
        }
    }
}