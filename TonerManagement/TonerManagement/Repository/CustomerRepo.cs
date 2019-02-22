using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagement.Repository
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly TonerManagementEntities _tonerManagementEntities;


        public CustomerRepo(DbContext tonerManagementEntities)
        {
            _tonerManagementEntities = (TonerManagementEntities) tonerManagementEntities;

        }

        public List<Customer> GetCustomersForUser(int userId)
        {
           
           //use to query user customer to get list of customer id's
           var customerIds = _tonerManagementEntities.UserCustomers.Where(uC => uC.userID == userId).Select(uC => uC.customerID);
           //get customers using customer id's from customer
           var customers = new List<Customer>();
           foreach (var customerId in customerIds)
           {
              customers.Add(_tonerManagementEntities.Customers.Find(customerId));
           }

           return customers;
        }

        public Customer GetCustomer(int customerId)
        {
            return _tonerManagementEntities.Customers.Find(customerId);
        }
    }
}