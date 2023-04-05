using AutoMapper;
using EuroBankAPI.Controllers;
using EuroBankAPI.DTOs;
using EuroBankAPI.Models;
using EuroBankAPI.Repository.IRepository;
using EuroBankAPI.Service.AuthService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
namespace EuroBankTest
{
    public class Tests
    {
        private EmployeeController _employeeController;
        private Mock<IUnitOfWork>  _unitOfWork;
        private Mock<ILogger<EmployeeController>> _logger;
        private Mock<IMapper> _mapper;
        private Mock<IAuthService> _authService;
        private List<Customer> _customers;
        [SetUp]
        public void Setup()
        {
            _unitOfWork= new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger<EmployeeController>>();
            _mapper = new Mock<IMapper>();
            _authService= new Mock<IAuthService>();

            _employeeController = new EmployeeController(_unitOfWork.Object, _logger.Object, _mapper.Object, _authService.Object);

            _customers = new List<Customer>();
            _customers.Add(new Customer()
            {
                DOB = DateTime.Today,
                CustomerId = "CustomerEurobank",
                Firstname = "Customer",
                Lastname = "Eurobank",
                EmailId = "Customer@gmail.com",
                PasswordHash = new byte[] { 44, 99, 229, 133, 22, 236, 120, 175, 219, 152, 102, 76, 191, 184, 5, 210, 222, 80, 252, 24, 134, 150, 254, 124, 199, 232, 88, 65, 129, 80, 143, 236, 94, 220, 203, 124, 200, 224, 105, 183, 16, 104, 192, 211, 33, 206, 166, 253, 119, 119, 32, 175, 117, 134, 114, 84, 157, 9, 16, 202, 173, 221, 141, 74 },
                PasswordSalt = new byte[] { 225, 123, 252, 79, 109, 166, 111, 44, 27, 233, 234, 50, 0, 12, 173, 77, 152, 172, 62, 38, 219, 131, 215, 151, 221, 90, 80, 30, 226, 39, 228, 104, 40, 181, 194, 174, 237, 170, 214, 85, 222, 187, 127, 210, 134, 245, 13, 214, 99, 82, 146, 169, 226, 220, 155, 47, 202, 125, 112, 131, 93, 154, 135, 109, 127, 84, 144, 69, 242, 12, 42, 98, 229, 215, 163, 211, 136, 61, 199, 51, 217, 93, 222, 120, 128, 107, 82, 84, 229, 143, 75, 219, 143, 111, 76, 130, 199, 54, 91, 128, 211, 7, 158, 2, 218, 17, 120, 228, 219, 157, 195, 160, 5, 211, 24, 118, 193, 190, 85, 228, 103, 103, 16, 255, 218, 166, 132, 175 },
                Address = "Chennai",
                Phone = "4242424242",
                PanNumber = "LBKTIOPNHW",
                CustomerCreationStatusId = 1
            });
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void GetAllCustomersTest()
        {
            Task<IEnumerable<Customer>> customers = Task.FromResult(_customers.AsEnumerable());
            _unitOfWork.Setup(p => p.Customers.GetAllAsync(null, null)).Returns(customers);
            var result = _employeeController.GetAllCustomers();
            Assert.That(result, Is.InstanceOf<Task<ActionResult<IEnumerable<CustomerDTO>>>>());
        }
    }
}