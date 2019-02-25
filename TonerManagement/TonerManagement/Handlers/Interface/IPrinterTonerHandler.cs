using System.Web.Mvc;
using TonerManagement.Models;

namespace TonerManagement.Handlers.Interface
{
    public interface IPrinterTonerHandler
    {
        ActionResult GetCoverage(CoverageForCompanyRequestModel coverageRequest, int userId);
    }
}