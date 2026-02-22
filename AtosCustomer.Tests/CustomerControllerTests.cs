using AtosCustomer.Api.Controllers;
using AtosCustomer.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using static AtosCustomer.Api.DTOs.CustomerModels;

namespace AtosCustomer.Tests
{
    public  class CustomerControllerTests
    {
        private readonly Mock<ICustomerRepository> _repoMock = new();
        private readonly Mock<ILogger<CustomersController>> _loggerMock = new();

        private CustomersController CreateController()
            => new CustomersController(_repoMock.Object, _loggerMock.Object);

        [Theory]
        [InlineData(null, "Doe")]
        [InlineData("John", null)]
        [InlineData("John", "  ")]
        [InlineData("", "Doe")]
        [InlineData("John", "")]
        public void Add_Should_Return_BadRequest_When_Invalid(string? firstname, string? surname)
        {
            var controller = CreateController();

            var result = controller.Add(new CreateCustomerRequest(firstname, surname));

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Add_Should_Return_Conflict_When_Duplicate()
        {
            _repoMock.Setup(r => r.Add("John", "Doe"))
                     .Returns((CustomerResponse?)null);

            var controller = CreateController();

            var result = controller.Add(new CreateCustomerRequest("John", "Doe"));

            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public void Add_Should_Return_201Created_When_Success()
        {
            _repoMock.Setup(r => r.Add("John", "Doe"))
                     .Returns(new CustomerResponse(1, "John", "Doe"));

            var controller = CreateController();

            var result = controller.Add(new CreateCustomerRequest("John", "Doe"));

            var objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal(201, objectResult.StatusCode);

            var customer = Assert.IsType<CustomerResponse>(objectResult.Value);
            Assert.Equal(1, customer.Id);
            Assert.Equal("John", customer.Firstname);
            Assert.Equal("Doe", customer.Surname);
        }

        [Fact]
        public void Remove_Should_Return_NotFound_When_Not_Exists()
        {
            _repoMock.Setup(r => r.Remove(1)).Returns(false);

            var controller = CreateController();

            var result = controller.Remove(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Remove_Should_Return_NoContent_When_Deleted()
        {
            _repoMock.Setup(r => r.Remove(1)).Returns(true);

            var controller = CreateController();

            var result = controller.Remove(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void GetAll_Should_Return_Ok_With_Customers()
        {
            _repoMock.Setup(r => r.GetAll())
                     .Returns(new List<CustomerResponse>
                     {
                     new(1, "A", "One")
                     });

            var controller = CreateController();

            var result = controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var customers = Assert.IsAssignableFrom<IEnumerable<CustomerResponse>>(okResult.Value);

            Assert.Single(customers);
        }
    }
}
