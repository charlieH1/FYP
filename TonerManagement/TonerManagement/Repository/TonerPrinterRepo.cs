using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

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
        public List<TonerPrinter> GetTonerPrinterForDevice(int printerId)
        {
            return _tonerManagementEntities.TonerPrinters.Where(tp => tp.printerId == printerId).ToList();
        }
        public List<TonerPrinter> GetTonerPrinterForDevice(int printerId, DateTime startDate, DateTime endDate)
        {
            return _tonerManagementEntities.TonerPrinters.Where(tp =>
                tp.printerId == printerId && tp.timestamp <= endDate && tp.timestamp >= startDate).ToList();
        }
    }
}