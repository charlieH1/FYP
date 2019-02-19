using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;
using TonerManagement.Toolsets;

namespace TonerManagement.Repository
{
    public class TonerPrinterRepo : ITonerPrinterRepo
    {
        private readonly TonerManagementEntities _tonerManagementEntities;
        public TonerPrinterRepo(DbContext tonerManagementEntities)
        {
            _tonerManagementEntities = (TonerManagementEntities) tonerManagementEntities;
        }
        public TonerPrinter GetTonerPrinter(int tonerPrinterId)
        {
            return _tonerManagementEntities.TonerPrinters.Find(tonerPrinterId);
        }
        public DbSet<TonerPrinter> GetAllTonerPrinters()
        {
            return _tonerManagementEntities.TonerPrinters;
        }
        public List<TonerPrinter> GetTonerPrinterForDevice(int printerId, CoverageToolset.ColorType color)
        {
            var tonerPrinters = _tonerManagementEntities.TonerPrinters.Where(tp => tp.printerId == printerId).ToList();
            switch (color)
            {
                case CoverageToolset.ColorType.C:
                    return tonerPrinters.Where(tp => tp.Toner.isCyan).ToList();
                case CoverageToolset.ColorType.Y:
                    return tonerPrinters.Where(tp => tp.Toner.isYellow).ToList();
                case CoverageToolset.ColorType.M:
                    return tonerPrinters.Where(tp => tp.Toner.isMagenta).ToList();
                case CoverageToolset.ColorType.K:
                    return tonerPrinters.Where(tp => tp.Toner.isKeying).ToList();
                default:
                    return null;
            }
        }
        
        public List<TonerPrinter> GetTonerPrinterForDevice(int printerId, DateTime startDate, DateTime endDate,
            CoverageToolset.ColorType color)
        {
            var tonerPrinters = _tonerManagementEntities.TonerPrinters.Where(tp =>
                tp.printerId == printerId && tp.timestamp <= endDate && tp.timestamp >= startDate);
            switch (color)
            {
                case CoverageToolset.ColorType.C:
                    return tonerPrinters.Where(tp => tp.Toner.isCyan).ToList();
                case CoverageToolset.ColorType.Y:
                    return tonerPrinters.Where(tp => tp.Toner.isYellow).ToList();
                case CoverageToolset.ColorType.M:
                    return tonerPrinters.Where(tp => tp.Toner.isMagenta).ToList();
                case CoverageToolset.ColorType.K:
                    return tonerPrinters.Where(tp => tp.Toner.isKeying).ToList();
                default:
                    return null;
            }
        }

        public List<TonerPrinter> GetTonerPrinterForCustomer(int customerId, DateTime startDate, DateTime endDate,
            CoverageToolset.ColorType color)
        {
            var tonerPrinters = _tonerManagementEntities.TonerPrinters.Where(tp =>
                tp.Printer.customerId==customerId && tp.timestamp <= endDate && tp.timestamp >= startDate);
            switch (color)
            {
                case CoverageToolset.ColorType.C:
                    return tonerPrinters.Where(tp => tp.Toner.isCyan).ToList();
                case CoverageToolset.ColorType.Y:
                    return tonerPrinters.Where(tp => tp.Toner.isYellow).ToList();
                case CoverageToolset.ColorType.M:
                    return tonerPrinters.Where(tp => tp.Toner.isMagenta).ToList();
                case CoverageToolset.ColorType.K:
                    return tonerPrinters.Where(tp => tp.Toner.isKeying).ToList();
                default:
                    return null;
            }
        }
    }
}