using AtosCustomer.Api.Entity;
using static AtosCustomer.Api.DTOs.CustomerModels;

namespace AtosCustomer.Api
{
    public static class CustomerMapping
    {
        public static CustomerResponse ToResponse(this Customer c)
        => new(c.Id, c.Firstname, c.Surname);
    }
}
