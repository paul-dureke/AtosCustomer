using static AtosCustomer.Api.DTOs.CustomerModels;

namespace AtosCustomer.Api.Repository
{
    public interface ICustomerRepository
    {
        CustomerResponse? Add(string firstname, string surname);
        bool Remove(int id);
        IReadOnlyCollection<CustomerResponse> GetAll();
    }
}
