using AtosCustomer.Api.Entity;
using System.Collections.Concurrent;
using static AtosCustomer.Api.DTOs.CustomerModels;

namespace AtosCustomer.Api.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ConcurrentDictionary<int, Customer> _store = new();
        private readonly object _lock = new();
        private int _lastId = 0;

        public Customer? Add(string firstname, string surname)
        {
            lock (_lock)
            {
                var exists = _store.Values.Any(c =>
                string.Equals(c.Firstname, firstname, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(c.Surname, surname, StringComparison.OrdinalIgnoreCase));

                if (exists)
                    return null;

                var id = ++_lastId;
                var customer = new Customer 
                { 
                    Id = id, 
                    Firstname = firstname, 
                    Surname = surname 
                };
                _store[id] = customer;

                return customer;
            }
        }

        public bool Remove(int id)
        {
            if (id <= 0) return false;

            lock (_lock)
            {
                if (!_store.TryRemove(id, out var removed))
                    return false;

                return true;
            }
        }

        public IReadOnlyCollection<Customer> GetAll()
        {
            return _store.Values.ToArray();
        }
    }
}
