using AutoMapper;
using EuroBankAPI.Models;
namespace EuroBankAPI.DTOs
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<Employee, EmployeeDTO>();
        }
    }
}
