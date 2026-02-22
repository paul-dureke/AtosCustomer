using AtosCustomer.Api.Entity;

namespace AtosCustomer.Api.Repository
{
    public interface ICustomerRepository
    {
        Customer? Add(string firstname, string surname);
        bool Remove(int id);
        IReadOnlyCollection<Customer> GetAll();
    }
}
