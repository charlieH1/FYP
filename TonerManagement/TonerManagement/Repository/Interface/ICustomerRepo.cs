using System.Collections.Generic;
using TonerManagement.Models;

namespace TonerManagement.Repository.Interface
{
    public interface ICustomerRepo
    {
        Customer GetCustomer(int customerId);
        List<Customer> GetCustomersForUser(int userId);
        bool UpdateCustomer(UpdateCustomerModel request);
    }
}