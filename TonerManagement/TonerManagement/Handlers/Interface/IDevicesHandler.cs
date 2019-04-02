using System.Web.Mvc;
using TonerManagement.Models;

namespace TonerManagement.Handlers.Interface
{
    public interface IDevicesHandler
    {
        ActionResult GetDeviceDetails(int printerId, string userName);
        ActionResult UpdateTonerLowOnDevice(TonerPercentageAndPrinterIdModel request, string userName);
    }
}