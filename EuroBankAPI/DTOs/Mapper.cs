using AutoMapper;
using EuroBankAPI.Models;

namespace EuroBankAPI.DTOs
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<CustomerCreationStatus, CustomerCreationStatusDTO>().ReverseMap();
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<Transaction, TransactionDTO>().ReverseMap();
            CreateMap<RefTransactionStatus,RefTransactionStatusDTO>().ReverseMap();
            CreateMap<RefTransactionType,RefTransactionTypeDTO>().ReverseMap();
            CreateMap<Models.Service, ServiceDTO>().ReverseMap();
            CreateMap<RefPaymentMethod, RefPaymentMethodDTO>().ReverseMap();
            CreateMap<CounterParty, CounterPartyDTO>().ReverseMap();
        }
    }
}
