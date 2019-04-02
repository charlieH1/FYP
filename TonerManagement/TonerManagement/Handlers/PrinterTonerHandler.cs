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
        private readonly IUserRepo _userRepo;

        public PrinterTonerHandler(ICoverageToolset coverageToolset, ICustomerRepo customerRepo,IPrinterRepo printerRepo,ITonerPrinterRepo tonerPrinterRepo, IUserRepo userRepo)
        {
            _coverageToolset = coverageToolset;
            _customerRepo = customerRepo;
            _printerRepo = printerRepo;
            _tonerPrinterRepo = tonerPrinterRepo;
            _userRepo = userRepo;
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
                        "An error occured, the daily coverage for colors do not pair up");

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
                        "An error occured, the daily coverage for all do not pair up");

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

        public ActionResult GetCoverageForPrinter(CoverageForPrinterRequestModel coverageRequest, int userId)
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
                    Data = _coverageToolset.GetListOfCoverageMonthly(coverageRequest.StartDate, coverageRequest.EndDate,coverageRequest.PrinterId, CoverageToolset.ColorType.C),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            else if (coverageRequest.CoverageType == coverageTypes[1])
                return new JsonResult
                {
                    ContentType = null,
                    ContentEncoding = null,
                    Data = _coverageToolset.GetListOfCoverageMonthly(coverageRequest.StartDate, coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.Y),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            else if (coverageRequest.CoverageType == coverageTypes[2])
                return new JsonResult
                {
                    ContentEncoding = null,
                    ContentType = null,
                    Data = _coverageToolset.GetListOfCoverageMonthly(coverageRequest.StartDate, coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.M),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            else if (coverageRequest.CoverageType == coverageTypes[3])
                return new JsonResult
                {
                    ContentEncoding = null,
                    ContentType = null,
                    Data = _coverageToolset.GetListOfCoverageMonthly(coverageRequest.StartDate, coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.K),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            else if (coverageRequest.CoverageType == coverageTypes[4])
            {
                var cCoverage = _coverageToolset.GetListOfCoverageMonthly(coverageRequest.StartDate,
                    coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.C);
                var yCoverage = _coverageToolset.GetListOfCoverageMonthly(coverageRequest.StartDate,
                    coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.Y);
                var mCoverage = _coverageToolset.GetListOfCoverageMonthly(coverageRequest.StartDate,
                    coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.M);
                if (cCoverage.Count != yCoverage.Count && cCoverage.Count != mCoverage.Count)
                    return new HttpStatusCodeResult(500,
                        "An error occured, the monthly coverage for colors do not pair up");

                var averageColorCoverage = new List<CoverageDateModel>();
                for (var i = 0; i < cCoverage.Count; i++)
                {
                    var averageCoverage = Math.Round((cCoverage[i].Coverage + yCoverage[i].Coverage + mCoverage[i].Coverage) / 3, 2, MidpointRounding.AwayFromZero);
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

            else if (coverageRequest.CoverageType == coverageTypes[5])
            {
                var cCoverage = _coverageToolset.GetListOfCoverageMonthly(coverageRequest.StartDate,
                    coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.C);
                var yCoverage = _coverageToolset.GetListOfCoverageMonthly(coverageRequest.StartDate,
                    coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.Y);
                var mCoverage = _coverageToolset.GetListOfCoverageMonthly(coverageRequest.StartDate,
                    coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.M);
                var kCoverage = _coverageToolset.GetListOfCoverageMonthly(coverageRequest.StartDate,
                    coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.K);
                if (cCoverage.Count != yCoverage.Count && cCoverage.Count != mCoverage.Count && cCoverage.Count != kCoverage.Count)
                    return new HttpStatusCodeResult(500,
                        "An error occured, the monthly coverage for all do not pair up");

                var averageCoverageAll = new List<CoverageDateModel>();
                for (var i = 0; i < cCoverage.Count; i++)
                {
                    var averageCoverage = Math.Round((cCoverage[i].Coverage + yCoverage[i].Coverage + mCoverage[i].Coverage + kCoverage[i].Coverage) / 4, 2, MidpointRounding.AwayFromZero);
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
                    Data = _coverageToolset.GetListOfCoverageDaily(coverageRequest.StartDate, coverageRequest.EndDate,coverageRequest.PrinterId, CoverageToolset.ColorType.C),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            else if (coverageRequest.CoverageType == coverageTypes[7])
                return new JsonResult
                {
                    ContentEncoding = null,
                    ContentType = null,
                    Data = _coverageToolset.GetListOfCoverageDaily(coverageRequest.StartDate, coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.Y),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            else if (coverageRequest.CoverageType == coverageTypes[8])
                return new JsonResult
                {
                    ContentEncoding = null,
                    ContentType = null,
                    Data = _coverageToolset.GetListOfCoverageDaily(coverageRequest.StartDate, coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.M),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            else if (coverageRequest.CoverageType == coverageTypes[9])
            {
                return new JsonResult
                {
                    ContentEncoding = null,
                    ContentType = null,
                    Data = _coverageToolset.GetListOfCoverageDaily(coverageRequest.StartDate, coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.K),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            else if (coverageRequest.CoverageType == coverageTypes[10])
            {
                var cCoverage = _coverageToolset.GetListOfCoverageDaily(coverageRequest.StartDate,
                    coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.C);
                var yCoverage = _coverageToolset.GetListOfCoverageDaily(coverageRequest.StartDate,
                    coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.Y);
                var mCoverage = _coverageToolset.GetListOfCoverageDaily(coverageRequest.StartDate,
                    coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.M);
                if (cCoverage.Count != yCoverage.Count && cCoverage.Count != mCoverage.Count)
                    return new HttpStatusCodeResult(500,
                        "An error occured, the monthly coverage for colors do not pair up");

                var averageColorCoverage = new List<CoverageDateModel>();
                for (var i = 0; i < cCoverage.Count; i++)
                {
                    var averageCoverage = Math.Round((cCoverage[i].Coverage + yCoverage[i].Coverage + mCoverage[i].Coverage) / 3, 2, MidpointRounding.AwayFromZero);
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
            else if (coverageRequest.CoverageType == coverageTypes[11])
            {
                var cCoverage = _coverageToolset.GetListOfCoverageDaily(coverageRequest.StartDate,
                    coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.C);
                var yCoverage = _coverageToolset.GetListOfCoverageDaily(coverageRequest.StartDate,
                    coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.Y);
                var mCoverage = _coverageToolset.GetListOfCoverageDaily(coverageRequest.StartDate,
                    coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.M);
                var kCoverage = _coverageToolset.GetListOfCoverageDaily(coverageRequest.StartDate,
                    coverageRequest.EndDate, coverageRequest.PrinterId, CoverageToolset.ColorType.K);
                if (cCoverage.Count != yCoverage.Count && cCoverage.Count != mCoverage.Count && cCoverage.Count != kCoverage.Count)
                    return new HttpStatusCodeResult(500,
                        "An error occured, the daily coverage for all do not pair up");

                var averageColorCoverage = new List<CoverageDateModel>();
                for (var i = 0; i < cCoverage.Count; i++)
                {
                    var averageCoverage = Math.Round((cCoverage[i].Coverage + yCoverage[i].Coverage + mCoverage[i].Coverage + kCoverage[i].Coverage) / 4, 2, MidpointRounding.AwayFromZero);
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

        public ActionResult GetCoverageGridForCustomer(int userId, int customerId)
        {
            if (_customerRepo.GetCustomer(customerId) == null) return new HttpStatusCodeResult(422);

            if (!_customerRepo.GetCustomersForUser(userId).Select(c => c.customerID)
                .Contains(customerId))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            
            var monthAgo = DateTime.Today.AddMonths(-1);
            var sixMonthAgo = DateTime.Today.AddMonths(-6);
            var yearAgo = DateTime.Today.AddYears(-1);
            var printerIds = new List<int>();
            foreach (var printer in _printerRepo.GetPrintersFromCustomer(customerId))
            {
                printerIds.Add(printer.printerId);
            }

            var coverageModels = new List<CoverageGridModel>();

            foreach (var printerId in printerIds)
            {
                var printer = _printerRepo.GetPrinter(printerId);
                CoverageGridModel coverageModel;
                if (printer.isColour)
                {
                    coverageModel = new CoverageGridModel
                    {

                        PrinterId = printerId,
                        CurrentCoverage = Math.Round(
                            (_coverageToolset.CalculateAverageCoverageForWholeLife(printerId,
                                 CoverageToolset.ColorType.C) +
                             _coverageToolset.CalculateAverageCoverageForWholeLife(printerId,
                                 CoverageToolset.ColorType.Y) +
                             _coverageToolset.CalculateAverageCoverageForWholeLife(printerId,
                                 CoverageToolset.ColorType.M) +
                             _coverageToolset.CalculateAverageCoverageForWholeLife(printerId,
                                 CoverageToolset.ColorType.K)
                            ) /
                            4.0, 2, MidpointRounding.AwayFromZero),
                        MonthCoverage = Math.Round(
                            (_coverageToolset.GetListOfCoverageMonthly(monthAgo, monthAgo, printerId,
                                 CoverageToolset.ColorType.C)[0].Coverage +
                             _coverageToolset.GetListOfCoverageMonthly(monthAgo, monthAgo, printerId,
                                 CoverageToolset.ColorType.Y)[0].Coverage +
                             _coverageToolset.GetListOfCoverageMonthly(monthAgo, monthAgo, printerId,
                                 CoverageToolset.ColorType.M)[0].Coverage +
                             _coverageToolset.GetListOfCoverageMonthly(monthAgo, monthAgo, printerId,
                                 CoverageToolset.ColorType.K)[0].Coverage) /
                            4.0, 2, MidpointRounding.AwayFromZero),
                        SixMonthCoverage = Math.Round(
                            (_coverageToolset.GetListOfCoverageMonthly(sixMonthAgo, sixMonthAgo, printerId,
                                 CoverageToolset.ColorType.C)[0].Coverage +
                             _coverageToolset.GetListOfCoverageMonthly(sixMonthAgo, sixMonthAgo, printerId,
                                 CoverageToolset.ColorType.Y)[0].Coverage +
                             _coverageToolset.GetListOfCoverageMonthly(sixMonthAgo, sixMonthAgo, printerId,
                                 CoverageToolset.ColorType.M)[0].Coverage +
                             _coverageToolset.GetListOfCoverageMonthly(sixMonthAgo, sixMonthAgo, printerId,
                                 CoverageToolset.ColorType.K)[0].Coverage) /
                            4.0, 2, MidpointRounding.AwayFromZero),
                        YearCoverage = Math.Round(
                            (_coverageToolset.GetListOfCoverageMonthly(yearAgo, yearAgo, printerId,
                                 CoverageToolset.ColorType.C)[0].Coverage +
                             _coverageToolset.GetListOfCoverageMonthly(yearAgo, yearAgo, printerId,
                                 CoverageToolset.ColorType.Y)[0].Coverage +
                             _coverageToolset.GetListOfCoverageMonthly(yearAgo, yearAgo, printerId,
                                 CoverageToolset.ColorType.M)[0].Coverage +
                             _coverageToolset.GetListOfCoverageMonthly(yearAgo, yearAgo, printerId,
                                 CoverageToolset.ColorType.K)[0].Coverage) /
                            4.0, 2, MidpointRounding.AwayFromZero)
                    };
                }
                else
                {
                    coverageModel = new CoverageGridModel
                    {

                        PrinterId = printerId,
                        CurrentCoverage = Math.Round(_coverageToolset.CalculateAverageCoverageForWholeLife(printerId,CoverageToolset.ColorType.K), 2, MidpointRounding.AwayFromZero),
                        MonthCoverage = Math.Round(_coverageToolset.GetListOfCoverageMonthly(monthAgo, monthAgo, printerId,CoverageToolset.ColorType.K)[0].Coverage, 2, MidpointRounding.AwayFromZero),
                        SixMonthCoverage = Math.Round(_coverageToolset.GetListOfCoverageMonthly(sixMonthAgo, sixMonthAgo, printerId,CoverageToolset.ColorType.K)[0].Coverage, 2, MidpointRounding.AwayFromZero),
                        YearCoverage = Math.Round(_coverageToolset.GetListOfCoverageMonthly(yearAgo, yearAgo, printerId,CoverageToolset.ColorType.K)[0].Coverage, 2, MidpointRounding.AwayFromZero)
                    };
                }


                coverageModels.Add(coverageModel);

            }

            return new JsonResult()
            {
                ContentType = null,
                ContentEncoding = null,
                Data = coverageModels,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult GetCurrentTonerLevel(int printerId, string userName)
        {
            var printer = _printerRepo.GetPrinter(printerId);
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

            var model = new TonerPercentageAndPrinterIdModel {PrinterID = printerId};
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

            var jsonResult = new JsonResult()
            {
                Data = model,
                ContentType = null,
                ContentEncoding = null,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            return jsonResult;

        }

    }
}