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

        [Fact]
        public void Remove_Should_Return_True_When_Existing()
        {
            var customer = _repo.Add("Jane", "Doe");

            var removed = _repo.Remove(customer!.Id);

            Assert.True(removed);
        }

        [Fact]
        public void Remove_Should_Return_False_When_Not_Existing()
        {
            var removed = _repo.Remove(999);

            Assert.False(removed);
        }

        [Fact]
        public void GetAll_Should_Return_All_Customers()
        {
            _repo.Add("A", "One");
            _repo.Add("B", "Two");

            var customers = _repo.GetAll();

            Assert.Equal(2, customers.Count);
        }
    }
}