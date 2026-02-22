using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AtosCustomer.Tests
{
    public class IntegrationTests
    {
        private HttpClient CreateClient()
        {
            var factory = new TestApiFactory();
            return factory.CreateClient();
        }

        [Fact]
        public async Task Post_Customers_WithValidData_Returns201Created_WithCustomerBody()
        {
            // Arrange
            var client = CreateClient();
            var request = new CreateCustomerRequest("John", "Doe");

            // Act
            var response = await client.PostAsJsonAsync("/api/customers", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var body = await response.Content.ReadFromJsonAsync<CustomerResponse>();
            Assert.NotNull(body);
            Assert.True(body!.Id > 0);
            Assert.Equal("John", body.Firstname);
            Assert.Equal("Doe", body.Surname);

            Assert.Null(response.Headers.Location);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Post_Customers_MissingFirstname_Returns400BadRequest(string? firstname)
        {
            var _client = CreateClient();
            var request = new CreateCustomerRequest(firstname, "Doe");

            var response = await _client.PostAsJsonAsync("/api/customers", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Post_Customers_MissingSurname_Returns400BadRequest(string? surname)
        {
            var _client = CreateClient();
            var request = new CreateCustomerRequest("John", surname);

            var response = await _client.PostAsJsonAsync("/api/customers", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_Customers_WithDuplicateName_Returns409Conflict()
        {
            //Arrange
            var _client = CreateClient();
            var request = new CreateCustomerRequest("John", "Doe");
            await _client.PostAsJsonAsync("/api/customers", request);

            //Act
            var duplicateResponse = await _client.PostAsJsonAsync("/api/customers", request);

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, duplicateResponse.StatusCode);
        }


        [Fact]
        public async Task Remove_Customers_WithExistingId_Returns204NoContent()
        {
            //Arrange: create a customer first
            var _client = CreateClient();
            var createResponse = await _client.PostAsJsonAsync("/api/customers",
                new CreateCustomerRequest("Jane", "Doe"));

            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            var created = await createResponse.Content.ReadFromJsonAsync<CustomerResponse>();
            Assert.NotNull(created);
            Assert.True(created!.Id > 0);

            //Act
            var deleteResponse = await _client.DeleteAsync($"/api/customers/{created.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }

        [Fact]
        public async Task Remove_Customers_WithNonExistingId_Returns404NotFound()
        {
            //Act
            var _client = CreateClient();
            var response = await _client.DeleteAsync("/api/customers/999999");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-999)]
        public async Task Remove_Customers_WithInvalidId_Returns400BadRequest(int id)
        {
            //Act
            var _client = CreateClient();
            var response = await _client.DeleteAsync($"/api/customers/{id}");

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task Get_Customers_WhenNoneExist_Returns200WithEmptyList()
        {
            //Act
            var _client = CreateClient();
            var response = await _client.GetAsync("/api/customers");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var customers = await response.Content.ReadFromJsonAsync<List<CustomerResponse>>();
            Assert.NotNull(customers);
            Assert.Empty(customers!);
        }

        [Fact]
        public async Task Get_Customers_ReturnsAllCreatedCustomers()
        {
            //Arrange
            var _client = CreateClient();
            await _client.PostAsJsonAsync("/api/customers",
                new CreateCustomerRequest("Eden", "Hazard"));

            await _client.PostAsJsonAsync("/api/customers",
                new CreateCustomerRequest("John", "Terry"));

            //Act
            var response = await _client.GetAsync("/api/customers");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var customers = await response.Content.ReadFromJsonAsync<List<CustomerResponse>>();
            Assert.NotNull(customers);

            Assert.Equal(2, customers!.Count);

            Assert.Contains(customers, c => c.Firstname == "Eden" && c.Surname == "Hazard");
            Assert.Contains(customers, c => c.Firstname == "John" && c.Surname == "Terry");
        }

        public record CreateCustomerRequest(string? Firstname, string? Surname);
        public record CustomerResponse(int Id, string Firstname, string Surname);
    }
}
