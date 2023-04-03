using AutoMapper;
using EuroBankAPI.DTOs;

namespace EuroBankAPI.Models
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<CustomerCreationStatus, CustomerCreationStatusDTO>().ReverseMap();
        }
    }
}
