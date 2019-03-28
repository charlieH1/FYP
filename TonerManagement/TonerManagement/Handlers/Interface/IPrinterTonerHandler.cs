using System.Web.Mvc;
using TonerManagement.Models;

namespace TonerManagement.Handlers.Interface
{
    public interface IPrinterTonerHandler
    {
        ActionResult GetCoverage(CoverageForCompanyRequestModel coverageRequest, int userId);
        ActionResult GetCoverageForPrinter(CoverageForPrinterRequestModel coverageRequest, int userId);
        ActionResult GetLowTonerLevelsOfCustomerPrinters(int customerId, int userId);
        ActionResult GetCoverageGridForCustomer(int userId, int customerId);
    }
}