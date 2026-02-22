using AtosCustomer.Api.Repository;

namespace AtosCustomer.Tests
{
    public class CustomerRepositoryTests
    {
        private readonly CustomerRepository _repo = new();

        [Fact]
        public void Add_Should_Create_Customer_With_Incremented_Id()
        {
            var customer = _repo.Add("John", "Doe");

            Assert.NotNull(customer);
            Assert.Equal(1, customer!.Id);
            Assert.Equal("John", customer.Firstname);
            Assert.Equal("Doe", customer.Surname);
        }

        [Fact]
        public void Add_Should_Return_Null_When_Duplicate()
        {
            _repo.Add("John", "Doe");

            var duplicate = _repo.Add("John", "Doe");

            Assert.Null(duplicate);
        }
    }
}