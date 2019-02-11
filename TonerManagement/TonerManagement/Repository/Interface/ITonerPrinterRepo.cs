using System;
using System.Collections.Generic;
using System.Data.Entity;
using TonerManagement.Models;

namespace TonerManagement.Repository.Interface
{
    public interface ITonerPrinterRepo
    {
        DbSet<TonerPrinter> GetAllTonerPrinters();
        TonerPrinter GetTonerPrinter(int tonerPrinterId);
        List<TonerPrinter> GetTonerPrinterForDevice(int printerId);
        List<TonerPrinter> GetTonerPrinterForDevice(int printerId, DateTime startDate, DateTime endDate);
    }
}