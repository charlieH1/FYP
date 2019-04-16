using System.Web.Mvc;
using TonerManagement.Models;

namespace TonerManagement.Handlers.Interface
{
    public interface ICustomerHandler
    {
        ActionResult GetCustomersForUser(string userName);
        ActionResult GetCustomer(int customerId, string userName);
        ActionResult UpdateCustomer(UpdateCustomerModel request, string userName);
    }
}