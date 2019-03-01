using System.Web.Mvc;

namespace TonerManagement.Handlers
{
    public interface ICustomerHandler
    {
        ActionResult GetCustomersForUser(string userName);
    }
}