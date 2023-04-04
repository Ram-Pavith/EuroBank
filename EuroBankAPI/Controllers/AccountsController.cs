﻿using AutoMapper;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Data.Common;

namespace EuroBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUnitOfWork _uw;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepo;
        public AccountsController(IUnitOfWork uw, IMapper mapper, IAccountRepository _accRepo)
        {
            _uw = uw;
            _mapper = mapper;
            _accountRepo = _accRepo;
        }


        [HttpPost]
        [Authorize(Roles = "Employee")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AccountCreationStatusDTO>> createAccount(string CustomerId)
        {
            //Checking if the customer exist
            var CustomerExists = await _uw.Customers.GetAsync(x => x.CustomerId == CustomerId);
            if (CustomerExists == null)
            {
                return NotFound();
            }
            else
            {
                //Checking if the customer already has an acocunt of Type "Current"
                var AccountExists = await _uw.Accounts.GetAsync(x => x.CustomerId == CustomerId && x.AccountTypeId == 2);
                if (AccountExists == null)
                {
                    return BadRequest();
                }
                else
                {
                    //Creating a new account 
                    Account newAcc = new Account();
                    newAcc.CustomerId = CustomerId;
                    newAcc.AccountTypeId = 2;   //current account
                    newAcc.AccountCreationStatusId = 1;     // success status

                    try
                    {
                        await _uw.Accounts.CreateAsync(newAcc);
                        AccountCreationStatus acs = await _uw.AccountCreationStatuses.GetAsync(x => x.AccountCreationStatusId == newAcc.AccountCreationStatusId);
                        AccountCreationStatusDTO accCreationStatusDTO = _mapper.Map<AccountCreationStatusDTO>(acs);
                        return accCreationStatusDTO;
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


        [HttpGet("getCustomerAccounts/{CustomerId}")]
        [Authorize(Roles = "Employee, Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AccountBalanceDTO>>> getCustomerAccounts(string CustomerId)
        {
            //Checking if the customer exist
            var CustomerExists = await _uw.Customers.GetAsync(x => x.CustomerId == CustomerId);
            if (CustomerExists == null)
            {
                return NotFound();
            }
            else
            {
                IEnumerable<Account> customerAccounts = await _uw.Accounts.GetAllAsync(x => x.CustomerId == CustomerId);
                //IEnumerable<AccountBalanceDTO> customerAccountsDTO = _mapper.Map<IEnumerable<AccountBalanceDTO>>(customerAccounts); --error
                List<AccountBalanceDTO> customerAccountsDTO = _mapper.Map<List<AccountBalanceDTO>>(customerAccounts);
                return customerAccountsDTO;
            }
        }

        [HttpGet("getAccount/{AccountId}")]
        [Authorize(Roles = "Employee, Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AccountBalanceDTO>> getAccount(Guid AccountId)
        {
            //Checking if account exist
            Account targetAccount = await _uw.Accounts.GetAsync(x => x.AccountId == AccountId);
            if (targetAccount == null)
            {
                return NotFound();
            }
            else
            {
                AccountBalanceDTO targetAccountDTO = _mapper.Map<AccountBalanceDTO>(targetAccount);
                return targetAccountDTO;
            }
        }

        [HttpGet("GetStatement/{AccountId}/{from_date}/{to_date}")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StatementDTO>>> getAccountStatement(Guid AccountId, DateTime? from_date, DateTime? to_date)
        {
            //Checking if account exist
            Account targetAccount = await _uw.Accounts.GetAsync(x => x.AccountId == AccountId);
            if (targetAccount == null)
            {
                return NotFound();
            }
            else
            {
                if (from_date != null && to_date != null)
                {
                    IEnumerable<Statement> stmt = await _uw.Statements.GetAllAsync(x => x.AccountId == AccountId &&
                                                                        x.Date >= from_date && x.Date <= to_date);
                    List<StatementDTO> AccountStatement = _mapper.Map<List<StatementDTO>>(stmt);
                    return AccountStatement;
                }
                else
                {
                    IEnumerable<Statement> stmt = await _uw.Statements.GetAllAsync(x => x.AccountId == AccountId &&
                                                                        x.Date.Month == DateTime.Now.Month);
                    List<StatementDTO> AccountStatement = _mapper.Map<List<StatementDTO>>(stmt);
                    return AccountStatement;
                }
            }

        }


        [HttpPost("deposit/{AccountId}/{amount}")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TransactionStatusDTO>> deposit(Guid AccountId, double amount)
        {
            //Fetching balance of the account
            Account targetAccount = await _uw.Accounts.GetAsync(x => x.AccountId == AccountId);
            double targetBalance = targetAccount.Balance;

            //Deposit limit: 1 lakh
            if (amount > 0 && amount <= 100000)
            {
                //On Success 
                TransactionStatus ts_success = new TransactionStatus();
                ts_success.TransactionStatusId = 1;
                ts_success.AccountId = AccountId;
                ts_success.SourceBalance = targetBalance + amount;
                ts_success.Message = "Successful deposit: " + "₹ " + Math.Round(amount, 2).ToString();
                //Updating Account Balance
                targetAccount.Balance = ts_success.SourceBalance;
                try
                {
                    await _accountRepo.UpdateAsync(targetAccount);
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
                TransactionStatusDTO tsSuccessDTO = _mapper.Map<TransactionStatusDTO>(ts_success);
                return tsSuccessDTO;
            }
            else
            {
                //On Failure
                TransactionStatus ts_failure = new TransactionStatus();
                ts_failure.TransactionStatusId = 2;
                ts_failure.AccountId = AccountId;
                ts_failure.SourceBalance = targetBalance;
                ts_failure.Message = "Failed deposit: " + "₹ " + Math.Round(amount, 2).ToString();
                TransactionStatusDTO tsFailureDTO = _mapper.Map<TransactionStatusDTO>(ts_failure);
                return tsFailureDTO;
            }
        }

        [HttpPost("withdraw/{AccountId}/{amount}")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TransactionStatusDTO>> withdraw(Guid AccountId, double amount)
        {
            //Fetching balance of the account
            Account targetAccount = await _uw.Accounts.GetAsync(x => x.AccountId == AccountId);
            double targetBalance = targetAccount.Balance;

            //On Success 
            if (amount <= targetBalance && amount >0 && amount <= 50000)    
            {
                TransactionStatus ts_success = new TransactionStatus();
                ts_success.TransactionStatusId = 1;
                ts_success.AccountId = AccountId;
                ts_success.SourceBalance = targetBalance - amount;
                ts_success.Message = "Successful withdraw: " + "₹ " + Math.Round(amount, 2).ToString();
                TransactionStatusDTO tsSuccessDTO = _mapper.Map<TransactionStatusDTO>(ts_success);

                //Updating Account Balance
                targetAccount.Balance = ts_success.SourceBalance;
                try
                {
                    await _accountRepo.UpdateAsync(targetAccount);
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

                return tsSuccessDTO;
            }
            //On exceeding withdrawal limit
            else if(amount >50000)
            {
                TransactionStatus ts_failure = new TransactionStatus();
                ts_failure.TransactionStatusId = 2;
                ts_failure.AccountId = AccountId;
                ts_failure.SourceBalance = targetBalance;
                ts_failure.Message = "Exceeded withdrawal limit: ₹50000";
                TransactionStatusDTO tsFailureDTO = _mapper.Map<TransactionStatusDTO>(ts_failure);
                return tsFailureDTO;
            }
            //On Insufficient balance
            else
            {
                TransactionStatus ts_failure = new TransactionStatus();
                ts_failure.TransactionStatusId = 2;
                ts_failure.AccountId = AccountId;
                ts_failure.SourceBalance = targetBalance;
                ts_failure.Message = "Insufficient balance";
                TransactionStatusDTO tsFailureDTO = _mapper.Map<TransactionStatusDTO>(ts_failure);
                return tsFailureDTO;
            }
                      
        }


    }
}
