using System.Web.Mvc;
using TonerManagement.Models;

namespace TonerManagement.Handlers.Interface
{
    public interface ILoginHandler
    {
        HttpStatusCodeResult LoginRequest(LoginModel login);
    }
}