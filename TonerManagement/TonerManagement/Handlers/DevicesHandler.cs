using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TonerManagement.Handlers.Interface;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;
using TonerManagement.Toolsets;
using TonerManagement.Toolsets.Interface;

namespace TonerManagement.Handlers
{
    public class DevicesHandler : IDevicesHandler
    {
        private readonly IPrinterRepo _printerRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly ICoverageToolset _coverageToolset;
        private readonly IUserRepo _userRepo;

        public DevicesHandler(IPrinterRepo printerRepo, ICustomerRepo customerRepo, ICoverageToolset coverageToolset, IUserRepo userRepo)
        {
            _printerRepo = printerRepo;
            _customerRepo = customerRepo;
            _coverageToolset = coverageToolset;
            _userRepo = userRepo;
        }

        public ActionResult GetDeviceDetails(int printerId, string userName)
        {
            var printer = _printerRepo.GetPrinter(printerId);
            if (printer == null)
            {
                return new HttpStatusCodeResult(404,"Printer couldn't be found");
            }

            var users = _userRepo.GetUsers(userName);
            if (users.Count == 0)
            {
                return new HttpStatusCodeResult(404, userName+ " User not found");
            }

            var user = users.First();
            var customer = _customerRepo.GetCustomer(printer.customerId);
            var customersForUser = _customerRepo.GetCustomersForUser(user.userId);
            if (!customersForUser.Contains(customer))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden,"User does not have access to this customer");
            }

            double coverage;
            if (printer.isColour)
            {
                coverage = Math.Round(
                    (_coverageToolset.CalculateAverageCoverageForWholeLife(printerId, CoverageToolset.ColorType.C) +
                     _coverageToolset.CalculateAverageCoverageForWholeLife(printerId, CoverageToolset.ColorType.Y) +
                     _coverageToolset.CalculateAverageCoverageForWholeLife(printerId, CoverageToolset.ColorType.M) +
                     _coverageToolset.CalculateAverageCoverageForWholeLife(printerId, CoverageToolset.ColorType.K)) / 4,
                    2,
                    MidpointRounding.AwayFromZero);
            }
            else
            {
                coverage = Math.Round(
                    _coverageToolset.CalculateAverageCoverageForWholeLife(printerId, CoverageToolset.ColorType.K), 2,
                    MidpointRounding.AwayFromZero);
            }

            var printerCoverageModel = new PrinterResponseModelWithAverageTonerCoverage()
            {
                PrinterId = printerId,
                AverageTonerCoverage = coverage,
                CyanLowTonerPercentage = printer.cyanLowPercentage,
                YellowLowTonerPercentage = printer.yellowLowPercentage,
                MagentaLowTonerPercentage = printer.magentaLowPercentage,
                KeyingLowTonerPercentage = printer.keyingLowPercentage,
                PrinterName = printer.printerName
            };

            var data = new {success = true, printer = printerCoverageModel, customer};

            var jsonResult = new JsonResult()
            {
                ContentType = null,
                ContentEncoding = null,
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            return jsonResult;

        }

        public ActionResult UpdateTonerLowOnDevice(TonerPercentageAndPrinterIdModel request, string userName)
        {
            var printer = _printerRepo.GetPrinter(request.PrinterID);
            if (printer == null)
            {
                return new HttpStatusCodeResult(404, "Printer couldn't be found");
            }

            var users = _userRepo.GetUsers(userName);
            if (users.Count == 0)
            {
                return new HttpStatusCodeResult(404, userName + " User not found");
            }

            var user = users.First();
            var customer = _customerRepo.GetCustomer(printer.customerId);
            var customersForUser = _customerRepo.GetCustomersForUser(user.userId);
            if (!customersForUser.Contains(customer))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User does not have access to this customer");
            }

            var toUpdate = new Printer
            {
                printerId = printer.printerId,
                customerId = printer.customerId,
                isColour = printer.isColour,
                printerName = printer.printerName,
                stockLocationId = printer.stockLocationId,
                cyanLowPercentage = request.Cyan,
                yellowLowPercentage = request.Yellow,
                magentaLowPercentage = request.Magenta,
                keyingLowPercentage = request.Keying
            };

            var success = _printerRepo.UpdatePrinter(toUpdate);
            return success ? new HttpStatusCodeResult(HttpStatusCode.OK) : new HttpStatusCodeResult(422,"Failed To update Db");
        }
    }
}