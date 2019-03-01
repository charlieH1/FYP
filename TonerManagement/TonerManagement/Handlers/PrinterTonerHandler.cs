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
    public class PrinterTonerHandler : IPrinterTonerHandler
    {
        private readonly ICoverageToolset _coverageToolset;
        private readonly ICustomerRepo _customerRepo;
        private readonly IPrinterRepo _printerRepo;
        private readonly ITonerPrinterRepo _tonerPrinterRepo;

        public PrinterTonerHandler(ICoverageToolset coverageToolset, ICustomerRepo customerRepo,IPrinterRepo printerRepo,ITonerPrinterRepo tonerPrinterRepo)
        {
            _coverageToolset = coverageToolset;
            _customerRepo = customerRepo;
            _printerRepo = printerRepo;
            _tonerPrinterRepo = tonerPrinterRepo;
        }

        public ActionResult GetCoverage(CoverageForCompanyRequestModel coverageRequest, int userId)
        {
            var coverageTypes = new string[12];
            coverageTypes[0] = "CyanMonthly";
            coverageTypes[1] = "YellowMonthly";
            coverageTypes[2] = "MagentaMonthly";
            coverageTypes[3] = "KeyingMonthly";
            coverageTypes[4] = "ColorMonthly";
            coverageTypes[5] = "AllMonthly";
            coverageTypes[6] = "CyanDaily";
            coverageTypes[7] = "YellowDaily";
            coverageTypes[8] = "MagentaDaily";
            coverageTypes[9] = "KeyingDaily";
            coverageTypes[10] = "ColorDaily";
            coverageTypes[11] = "AllDaily";
            if (!coverageTypes.Contains(coverageRequest.CoverageType)) return new HttpStatusCodeResult(422);

            if (_customerRepo.GetCustomer(coverageRequest.CustomerId) == null) return new HttpStatusCodeResult(422);

            if (!_customerRepo.GetCustomersForUser(userId).Select(c => c.customerID)
                .Contains(coverageRequest.CustomerId))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            if (coverageRequest.CoverageType == coverageTypes[0])
                return new JsonResult
                {
                    ContentEncoding = null,
                    ContentType = null,
                    Data = _coverageToolset.GetListOfCoverageMonthlyForCustomer(coverageRequest.CustomerId,
                        coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.C),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            else if (coverageRequest.CoverageType == coverageTypes[1])
                return new JsonResult
                {
                    ContentType = null,
                    ContentEncoding = null,
                    Data = _coverageToolset.GetListOfCoverageMonthlyForCustomer(coverageRequest.CustomerId,
                        coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.Y),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            else if (coverageRequest.CoverageType == coverageTypes[2])
                return new JsonResult
                {
                    ContentEncoding = null,
                    ContentType = null,
                    Data = _coverageToolset.GetListOfCoverageMonthlyForCustomer(coverageRequest.CustomerId,
                        coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.M),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            else if (coverageRequest.CoverageType == coverageTypes[3])
                return new JsonResult
                {
                    ContentEncoding = null,
                    ContentType = null,
                    Data = _coverageToolset.GetListOfCoverageMonthlyForCustomer(coverageRequest.CustomerId,
                        coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.K),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            else if (coverageRequest.CoverageType == coverageTypes[4])
            {
                var cCoverage = _coverageToolset.GetListOfCoverageMonthlyForCustomer(coverageRequest.CustomerId,
                    coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.C);
                var yCoverage = _coverageToolset.GetListOfCoverageMonthlyForCustomer(coverageRequest.CustomerId,
                    coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.Y);
                var mCoverage = _coverageToolset.GetListOfCoverageMonthlyForCustomer(coverageRequest.CustomerId,
                    coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.M);
                if (cCoverage.Count != yCoverage.Count && cCoverage.Count != mCoverage.Count)
                    return new HttpStatusCodeResult(500,
                        "An error occured, the monthly coverage for colors do not pair up");

                var averageColorCoverage = new List<CoverageDateModel>();
                for (var i = 0; i < cCoverage.Count; i++)
                {
                    var averageCoverage = Math.Round((cCoverage[i].Coverage + yCoverage[i].Coverage + mCoverage[i].Coverage) / 3,2,MidpointRounding.AwayFromZero);
                    averageColorCoverage.Add(new CoverageDateModel
                        {Coverage = averageCoverage, Date = cCoverage[i].Date});
                }

                return new JsonResult
                {
                    ContentEncoding = null,
                    ContentType = null,
                    Data = averageColorCoverage,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            else if (coverageRequest.CoverageType == coverageTypes[5])
            {
                var cCoverage = _coverageToolset.GetListOfCoverageMonthlyForCustomer(coverageRequest.CustomerId,
                    coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.C);
                var yCoverage = _coverageToolset.GetListOfCoverageMonthlyForCustomer(coverageRequest.CustomerId,
                    coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.Y);
                var mCoverage = _coverageToolset.GetListOfCoverageMonthlyForCustomer(coverageRequest.CustomerId,
                    coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.M);
                var kCoverage = _coverageToolset.GetListOfCoverageMonthlyForCustomer(coverageRequest.CustomerId,
                    coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.K);
                if (cCoverage.Count != yCoverage.Count && cCoverage.Count != mCoverage.Count&& cCoverage.Count!=kCoverage.Count)
                    return new HttpStatusCodeResult(500,
                        "An error occured, the monthly coverage for all do not pair up");

                var averageCoverageAll = new List<CoverageDateModel>();
                for (var i = 0; i < cCoverage.Count; i++)
                {
                    var averageCoverage = Math.Round((cCoverage[i].Coverage + yCoverage[i].Coverage + mCoverage[i].Coverage +kCoverage[i].Coverage) / 4,2,MidpointRounding.AwayFromZero);
                    averageCoverageAll.Add(new CoverageDateModel
                        { Coverage = averageCoverage, Date = cCoverage[i].Date });
                }

                return new JsonResult
                {
                    ContentEncoding = null,
                    ContentType = null,
                    Data = averageCoverageAll,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            else if (coverageRequest.CoverageType == coverageTypes[6])
                return new JsonResult
                {
                    ContentEncoding = null,
                    ContentType = null,
                    Data = _coverageToolset.GetListOfCoverageDailyForCustomer(coverageRequest.CustomerId,
                        coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.C),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            else if (coverageRequest.CoverageType == coverageTypes[7])
                return new JsonResult
                {
                    ContentEncoding = null,
                    ContentType = null,
                    Data = _coverageToolset.GetListOfCoverageDailyForCustomer(coverageRequest.CustomerId,
                        coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.Y),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            else if (coverageRequest.CoverageType == coverageTypes[8])
                return new JsonResult
                {
                    ContentEncoding = null,
                    ContentType = null,
                    Data = _coverageToolset.GetListOfCoverageDailyForCustomer(coverageRequest.CustomerId,
                        coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.M),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            else if (coverageRequest.CoverageType == coverageTypes[9])
            {
                return new JsonResult
                {
                    ContentEncoding = null,
                    ContentType = null,
                    Data = _coverageToolset.GetListOfCoverageDailyForCustomer(coverageRequest.CustomerId,
                        coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.K),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            else if (coverageRequest.CoverageType == coverageTypes[10])
            {
                var cCoverage = _coverageToolset.GetListOfCoverageDailyForCustomer(coverageRequest.CustomerId,
                    coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.C);
                var yCoverage = _coverageToolset.GetListOfCoverageDailyForCustomer(coverageRequest.CustomerId,
                    coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.Y);
                var mCoverage = _coverageToolset.GetListOfCoverageDailyForCustomer(coverageRequest.CustomerId,
                    coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.M);
                if (cCoverage.Count != yCoverage.Count && cCoverage.Count != mCoverage.Count)
                    return new HttpStatusCodeResult(500,
                        "An error occured, the monthly coverage for colors do not pair up");

                var averageColorCoverage = new List<CoverageDateModel>();
                for (var i = 0; i < cCoverage.Count; i++)
                {
                    var averageCoverage = Math.Round((cCoverage[i].Coverage + yCoverage[i].Coverage + mCoverage[i].Coverage) / 3,2,MidpointRounding.AwayFromZero);
                    averageColorCoverage.Add(new CoverageDateModel
                        {Coverage = averageCoverage, Date = cCoverage[i].Date});
                }

                return new JsonResult
                {
                    ContentEncoding = null,
                    ContentType = null,
                    Data = averageColorCoverage,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else if (coverageRequest.CoverageType == coverageTypes[11])
            {
                var cCoverage = _coverageToolset.GetListOfCoverageDailyForCustomer(coverageRequest.CustomerId,
                    coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.C);
                var yCoverage = _coverageToolset.GetListOfCoverageDailyForCustomer(coverageRequest.CustomerId,
                    coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.Y);
                var mCoverage = _coverageToolset.GetListOfCoverageDailyForCustomer(coverageRequest.CustomerId,
                    coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.M);
                var kCoverage = _coverageToolset.GetListOfCoverageDailyForCustomer(coverageRequest.CustomerId,
                    coverageRequest.StartDate, coverageRequest.EndDate, CoverageToolset.ColorType.K);
                if (cCoverage.Count != yCoverage.Count && cCoverage.Count != mCoverage.Count&& cCoverage.Count!=kCoverage.Count)
                    return new HttpStatusCodeResult(500,
                        "An error occured, the monthly coverage for all do not pair up");

                var averageColorCoverage = new List<CoverageDateModel>();
                for (var i = 0; i < cCoverage.Count; i++)
                {
                    var averageCoverage = Math.Round((cCoverage[i].Coverage + yCoverage[i].Coverage + mCoverage[i].Coverage+kCoverage[i].Coverage) / 4,2,MidpointRounding.AwayFromZero);
                    averageColorCoverage.Add(new CoverageDateModel
                        { Coverage = averageCoverage, Date = cCoverage[i].Date });
                }

                return new JsonResult
                {
                    ContentEncoding = null,
                    ContentType = null,
                    Data = averageColorCoverage,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            return new HttpStatusCodeResult(422);
        }

        public ActionResult GetLowTonerLevelsOfCustomerPrinters(int customerId, int userId)
        {
            var printers = _printerRepo.GetPrintersFromCustomer(customerId);
            var lowTonerLevelList = new List<TonerPercentageAndPrinterIdModel>();

            if (_customerRepo.GetCustomer(customerId) == null) return new HttpStatusCodeResult(422);

            if (!_customerRepo.GetCustomersForUser(userId).Select(c => c.customerID)
                .Contains(customerId))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            foreach (var printer in printers)
            {
                var model = new TonerPercentageAndPrinterIdModel {PrinterID = printer.printerId};
                var cyanLevels =
                    _tonerPrinterRepo.GetTonerPrinterForDevice(printer.printerId, CoverageToolset.ColorType.C);
                var yellowLevels =
                    _tonerPrinterRepo.GetTonerPrinterForDevice(printer.printerId, CoverageToolset.ColorType.Y);
                var magentaLevels =
                    _tonerPrinterRepo.GetTonerPrinterForDevice(printer.printerId, CoverageToolset.ColorType.M);
                var keyingLevels =
                    _tonerPrinterRepo.GetTonerPrinterForDevice(printer.printerId, CoverageToolset.ColorType.K);
                if (cyanLevels != null)
                {
                    model.Cyan = cyanLevels.OrderBy(tp => tp.timestamp).Last().tonerPercentage;
                }

                if (yellowLevels != null)
                {
                    model.Yellow = yellowLevels.OrderBy(tp => tp.timestamp).Last().tonerPercentage;
                }

                if (magentaLevels != null)
                {
                    model.Magenta = magentaLevels.OrderBy(tP => tP.timestamp).Last().tonerPercentage;
                }

                if (keyingLevels != null)
                {
                    model.Keying = keyingLevels.OrderBy(tP => tP.timestamp).Last().tonerPercentage;
                }

                if (model.Cyan <= printer.cyanLowPercentage || model.Yellow <= printer.yellowLowPercentage ||
                    model.Magenta <= printer.magentaLowPercentage || model.Keying <= printer.keyingLowPercentage)
                {
                    lowTonerLevelList.Add(model);
                }
            }

            return new JsonResult()
            {
                ContentType = null,
                ContentEncoding = null,
                Data = lowTonerLevelList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}