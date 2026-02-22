using System.Text.Json.Serialization;

namespace AtosCustomer.Api.DTOs
{
    public class CustomerModels
    {
        public record CreateCustomerRequest(
        [property: JsonPropertyName("firstname")] string? Firstname,
        [property: JsonPropertyName("surname")] string? Surname);
        public record CustomerResponse(int Id, string Firstname, string Surname);
    }
}
