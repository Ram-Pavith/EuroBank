﻿using AutoMapper;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using EuroBankAPI.Service.AuthService;
using Microsoft.AspNetCore.Authorization;
using EuroBankAPI.Service.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EuroBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public EmployeeController(IUnitOfWork context,ILogger<EmployeeController> logger ,IMapper mapper,IAuthService authService) { 
            _context= context;
            _logger= logger;
            _mapper= mapper;
            _authService= authService;
        }
        [HttpPost("EmployeeRegister")]
        //[Authorize(Roles = "Employee")]
        public async Task<ActionResult<Employee>> EmployeeRegister(EmployeeRegisterDTO employeeRegisterDTO)
        {
            try
            {
                var employeeDTO = _mapper.Map<EmployeeDTO>(employeeRegisterDTO);
                _authService.CreatePasswordHash(employeeRegisterDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
                employeeDTO.PasswordHash = passwordHash;
                employeeDTO.PasswordSalt = passwordSalt;
                Employee employee = _mapper.Map<Employee>(employeeDTO);
                await _context.Employees.CreateAsync(employee);
                return employee;
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<CustomerCreationStatusDTO>> CreateCustomer(CustomerRegisterDTO customerRegisterDTO)
        {
            var customerDTO = _mapper.Map<CustomerDTO>(customerRegisterDTO);
            _authService.CreatePasswordHash(customerRegisterDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
            customerDTO.PasswordHash = passwordHash;
            customerDTO.PasswordSalt = passwordSalt;
            try
            {
                Customer customer = _mapper.Map<Customer>(customerDTO);
                CustomerCreationStatus customerCreationStatus;
                try
                {
                    Account account = new Account()
                    {
                        DateCreated = DateTime.Now,
                        Balance = 10000
                    };
                    customerCreationStatus = new CustomerCreationStatus() { 
                        Message = "Success"
                    };
                    try
                    {
                        AccountCreationStatus accountCreationStatus = new AccountCreationStatus()
                        {
                            Message = "Success"
                        };
                        AccountType accountType = new AccountType()
                        {
                            Type= "Savings"
                        };

                        await _context.AccountCreationStatuses.CreateAsync(accountCreationStatus);
                        account.AccountCreationStatusId = accountCreationStatus.AccountCreationStatusId;
                        await _context.AccountTypes.CreateAsync(accountType);
                        account.AccountTypeId = accountType.AccountTypeId;
                        account.CustomerId = customer.CustomerId;
                        await _context.CustomerCreationStatuses.CreateAsync(customerCreationStatus);
                        customer.CustomerCreationStatusId = customerCreationStatus.CustomerCreationId;
                        customer.CustomerCreationStatusId = 1;
                        await _context.Customers.CreateAsync(customer);
                        await _context.Accounts.CreateAsync(account);


                    }
                    catch (Exception ex)
                    {
                        AccountCreationStatus accountCreationStatus = new AccountCreationStatus()
                        {
                            Message = "Failure"
                        };
                    }
                }catch(Exception ex) {
                    customerCreationStatus = new CustomerCreationStatus()
                    {
                        Message = "Failure"
                    };
                }
                CustomerCreationStatusDTO customerCreationStatusDTO = _mapper.Map<CustomerCreationStatusDTO>(customerCreationStatus);
                return customerCreationStatusDTO;
            
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /* [HttpPost]
         [Authorize(Roles = "Employee")]
         public async Task<Employee> Register(EmployeeDTO employeeDTO)
         {
             Employee employee = _mapper.Map<Employee>(employeeDTO);
             await _context.Employees.CreateAsync(employee);
             return employee;
         }*/
        [HttpPost("EmployeeLogin")]
        public async Task<ActionResult<UserAuthResponseDTO>> EmployeeLogin(EmployeeLoginDTO employeeLogin)
        {
            UserAuthResponseDTO response;
            try
            {
                var request = _mapper.Map<UserAuthLoginDTO>(employeeLogin);
                response = await _authService.LoginEmployeeAndCustomer(request);
                if (response.Success)
                    return Ok(response);
                else
                    return BadRequest(response.Message);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("ViewAllTransactions")]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> ViewAllTransaction()
        {
            try
            {
                var Transactions = await _context.Transactions.GetAllAsync();

                List<TransactionDTO> TransactionDTOs = _mapper.Map<List<TransactionDTO>>(Transactions);

                return TransactionDTOs;
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ViewAllBankAccounts")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> ViewAllBankAccounts()
        {
            try
            {
                var BankAccounts = await _context.Accounts.GetAllAsync();
                List<AccountDTO> AccountsDTOs = _mapper.Map<List<AccountDTO>>(BankAccounts);
                return AccountsDTOs;
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("ResetPassword")]
        public async Task<ActionResult<EmployeeDTO>> ResetPassword(string Email, string Password)
        {
            try
            {
                Employee employee = await _context.Employees.GetAsync(x => x.EmailId == Email);
                _authService.CreatePasswordHash(Password, out byte[] passwordHash, out byte[] passwordSalt);

                employee.PasswordHash = passwordHash;
                employee.PasswordSalt = passwordSalt;
                await _context.Employees.UpdateAsync(employee);

                EmployeeDTO employeeDTO = _mapper.Map<EmployeeDTO>(employee);
                return employeeDTO;
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
