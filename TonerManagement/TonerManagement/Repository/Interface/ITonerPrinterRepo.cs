using System;
using System.Collections.Generic;
using System.Data.Entity;
using TonerManagement.Models;
using TonerManagement.Toolsets;

namespace TonerManagement.Repository.Interface
{
    public interface ITonerPrinterRepo
    {
        DbSet<TonerPrinter> GetAllTonerPrinters();
        TonerPrinter GetTonerPrinter(int tonerPrinterId);
        List<TonerPrinter> GetTonerPrinterForDevice(int printerId, CoverageToolset.ColorType color);
        List<TonerPrinter> GetTonerPrinterForDevice(int printerId, DateTime startDate, DateTime endDate,
            CoverageToolset.ColorType color);
        List<TonerPrinter> GetTonerPrinterForCustomer(int customerId, DateTime startDate, DateTime endDate,
            CoverageToolset.ColorType color);
    }
}