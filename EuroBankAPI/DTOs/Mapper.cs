﻿using AutoMapper;
using EuroBankAPI.Models;

namespace EuroBankAPI.DTOs
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<CustomerCreationStatus, CustomerCreationStatusDTO>().ReverseMap();
            CreateMap<Transaction, TransactionDTO>().ReverseMap();
            CreateMap<RefTransactionStatus,RefTransactionStatusDTO>().ReverseMap();
            CreateMap<RefTransactionType,RefTransactionTypeDTO>().ReverseMap();
            CreateMap<Models.Service, ServiceDTO>().ReverseMap();
            CreateMap<RefPaymentMethod, RefPaymentMethodDTO>().ReverseMap();
            CreateMap<CounterParty, CounterPartyDTO>().ReverseMap();
        }
    }
}
