using System;
using System.Collections.Generic;
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
        private readonly ITonerPrinterRepo _tonerPrinterRepo;
        private readonly IStockLocationRepo _stockLocationRepo;

        public DevicesHandler(IPrinterRepo printerRepo, ICustomerRepo customerRepo, ICoverageToolset coverageToolset,
            IUserRepo userRepo, ITonerPrinterRepo tonerPrinterRepo, IStockLocationRepo stockLocationRepo)
        {
            _printerRepo = printerRepo;
            _customerRepo = customerRepo;
            _coverageToolset = coverageToolset;
            _userRepo = userRepo;
            _tonerPrinterRepo = tonerPrinterRepo;
            _stockLocationRepo = stockLocationRepo;
        }

        public ActionResult GetDetailedPrinterGrid(int customerId, string userName)
        {
            

            var users = _userRepo.GetUsers(userName);
            if (users.Count == 0)
            {
                return new HttpStatusCodeResult(404, userName + " User not found");
            }

            var user = users.First();
            var customer = _customerRepo.GetCustomer(customerId);
            if (customer == null)
            {
                return new HttpStatusCodeResult(404,"Customer not found");
            }
            var customersForUser = _customerRepo.GetCustomersForUser(user.userId);
            if (!customersForUser.Contains(customer))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User does not have access to this customer");
            }
            var printers = _printerRepo.GetPrintersFromCustomer(customerId);

            var detailedPrintersModel = new List<HighDetailPrinterModel>();

            foreach (var printer in printers)
            {
                if (printer.isColour)
                {

                    var cyanLevels = _tonerPrinterRepo
                        .GetTonerPrinterForDevice(printer.printerId, CoverageToolset.ColorType.C);
                    var yellowLevels = _tonerPrinterRepo
                        .GetTonerPrinterForDevice(printer.printerId, CoverageToolset.ColorType.Y);
                    var magentaLevels = _tonerPrinterRepo
                        .GetTonerPrinterForDevice(printer.printerId, CoverageToolset.ColorType.M);
                    var keyingLevels = _tonerPrinterRepo
                        .GetTonerPrinterForDevice(printer.printerId, CoverageToolset.ColorType.K);
                    var cyanLevel = cyanLevels.Count > 0 ? cyanLevels.OrderBy(tp => tp.timestamp).Last().tonerPercentage : 0;
                    var yellowLevel = yellowLevels.Count > 0 ? yellowLevels.OrderBy(tp => tp.timestamp).Last().tonerPercentage : 0;
                    var magentaLevel = magentaLevels.Count > 0 ? magentaLevels.OrderBy(tp => tp.timestamp).Last().tonerPercentage : 0;
                    var keyingLevel = keyingLevels.Count > 0 ? keyingLevels.OrderBy(tp => tp.timestamp).Last().tonerPercentage : 0;
                    
                    var highDetailPrinter = new HighDetailPrinterModel()
                    {
                        CyanCoverage =
                            _coverageToolset.CalculateAverageCoverageForWholeLife(printer.printerId,
                                CoverageToolset.ColorType.C),
                        YellowCoverage =
                            _coverageToolset.CalculateAverageCoverageForWholeLife(printer.printerId,
                                CoverageToolset.ColorType.Y),
                        MagentaCoverage =
                            _coverageToolset.CalculateAverageCoverageForWholeLife(printer.printerId,
                                CoverageToolset.ColorType.M),
                        KeyingCoverage =
                            _coverageToolset.CalculateAverageCoverageForWholeLife(printer.printerId,
                                CoverageToolset.ColorType.K),
                        AverageCoverage = Math.Round((_coverageToolset.CalculateAverageCoverageForWholeLife(printer.printerId,
                            CoverageToolset.ColorType.C)+ _coverageToolset.CalculateAverageCoverageForWholeLife(printer.printerId,
                                               CoverageToolset.ColorType.Y)+ _coverageToolset.CalculateAverageCoverageForWholeLife(printer.printerId,
                                               CoverageToolset.ColorType.M)+ _coverageToolset.CalculateAverageCoverageForWholeLife(printer.printerId,
                                               CoverageToolset.ColorType.K))/4,2,MidpointRounding.AwayFromZero),
                        CyanLevel = cyanLevel,
                        YellowLevel = yellowLevel,
                        MagentaLevel = magentaLevel,
                        KeyingLevel = keyingLevel,
                        PrinterId = printer.printerId,
                        PrinterName = printer.printerName,
                    };
                    detailedPrintersModel.Add(highDetailPrinter);
                }
                else
                {
                    var keyingLevels =
                        _tonerPrinterRepo.GetTonerPrinterForDevice(printer.printerId, CoverageToolset.ColorType.K);
                    var keyingLevel = keyingLevels.Count > 0 ? keyingLevels.OrderBy(tp => tp.timestamp).Last().tonerPercentage : 0;
                    var highDetailPrinter = new HighDetailPrinterModel()
                    {
                        KeyingCoverage =
                            _coverageToolset.CalculateAverageCoverageForWholeLife(printer.printerId,
                                CoverageToolset.ColorType.K),
                        AverageCoverage = _coverageToolset.CalculateAverageCoverageForWholeLife(printer.printerId,
                            CoverageToolset.ColorType.K),
                        KeyingLevel = keyingLevel,
                        PrinterId = printer.printerId,
                        PrinterName = printer.printerName,
                    };
                    detailedPrintersModel.Add(highDetailPrinter);
                }
                
            }
            var jsonResult = new JsonResult()
            {
                ContentType = null,
                ContentEncoding = null,
                Data = detailedPrintersModel,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            return jsonResult;
        }

        public ActionResult GetTonerPercentageAndIdsForPrintersPerStockLocation(int stockLocationId, string userName)
        {
            var stockLocation = _stockLocationRepo.GetStockLocation(stockLocationId);
            if (stockLocation == null)
            {
                return new HttpStatusCodeResult(404, "Stock Location could not be found");
            }

            var users = _userRepo.GetUsers(userName);
            if (users.Count == 0)
            {
                return new HttpStatusCodeResult(404, userName + " User not found");
            }

            var user = users.First();
            var customer = _customerRepo.GetCustomer(stockLocation.customerId);
            var customersForUser = _customerRepo.GetCustomersForUser(user.userId);
            if (!customersForUser.Contains(customer))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "User does not have access to this customer");
            }

            var details = new List<TonerIdTonerPercentageAndPrinterModel>();

            var printers = _printerRepo.GetPrinterFromStockLocation(stockLocationId);

            foreach (var printer in printers)
            {
                if (printer.isColour)
                {
                    var cyan = _tonerPrinterRepo.GetTonerPrinterForDevice(printer.printerId,
                        CoverageToolset.ColorType.C).OrderBy(tp => tp.timestamp).Last();
                    var yellow = _tonerPrinterRepo.GetTonerPrinterForDevice(printer.printerId,
                        CoverageToolset.ColorType.Y).OrderBy(tp => tp.timestamp).Last();
                    var magenta = _tonerPrinterRepo.GetTonerPrinterForDevice(printer.printerId,
                        CoverageToolset.ColorType.M).OrderBy(tp => tp.timestamp).Last();
                    var keying = _tonerPrinterRepo.GetTonerPrinterForDevice(printer.printerId,
                        CoverageToolset.ColorType.K).OrderBy(tp => tp.timestamp).Last();
                    var detail = new TonerIdTonerPercentageAndPrinterModel()
                    {
                        DeviceId = printer.printerId,
                        CyanPercentage = cyan.tonerPercentage,
                        CyanId = cyan.tonerId,
                        YellowPercentage = yellow.tonerPercentage,
                        YellowId = yellow.tonerId,
                        MagentaPercentage = magenta.tonerPercentage,
                        MagentaId = magenta.tonerId,
                        KeyingPercentage = keying.tonerPercentage,
                        KeyingId = keying.tonerId
                    };
                    details.Add(detail);
                }
                else
                {
                    var keying = _tonerPrinterRepo.GetTonerPrinterForDevice(printer.printerId,
                        CoverageToolset.ColorType.K).OrderBy(tp => tp.timestamp).Last();
                    var detail = new TonerIdTonerPercentageAndPrinterModel()
                    {
                        DeviceId = printer.printerId,
                        KeyingPercentage = keying.tonerPercentage,
                        KeyingId = keying.tonerId
                    };
                    details.Add(detail);
                }
            }

            var jsonResult = new JsonResult()
            {
                ContentType = null,
                ContentEncoding = null,
                Data = details,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            return jsonResult;

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