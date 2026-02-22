using System.Text.Json.Serialization;

namespace AtosCustomer.Api.DTOs
{
    public class CustomerModels
    {
        public record CreateCustomerRequest(
        [property: JsonPropertyName("firstname")] string? Firstname,
        [property: JsonPropertyName("surname")] string? Surname);

        public record CustomerResponse(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("firstname")] string Firstname,
        [property: JsonPropertyName("surname")] string Surname);
    }
}
