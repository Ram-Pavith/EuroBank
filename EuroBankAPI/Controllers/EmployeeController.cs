using AutoMapper;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EuroBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;
        public EmployeeController(IUnitOfWork context,ILogger<EmployeeController> logger ,IMapper mapper) { 
            _context= context;
            _logger= logger;
            _mapper= mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<Employee> Register(EmployeeDTO employeeDTO)
        {
            Employee employee = _mapper.Map<Employee>(employeeDTO);
            await _context.Employees.CreateAsync(employee);
            return employee;
        }


    }
}
