using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagement.Repository
{
    public class PrinterRepo : IPrinterRepo
    {
        private readonly TonerManagementEntities _tonerManagementEntities;
        public PrinterRepo(DbContext tonerManagementEntities)
        {
            _tonerManagementEntities = (TonerManagementEntities) tonerManagementEntities;
        }

        public List<Printer> GetPrintersFromCustomer(int customerId)
        {
            return _tonerManagementEntities.Printers.Where(p => p.customerId == customerId).ToList();
        }

        public Printer GetPrinter(int printerId)
        {
            return _tonerManagementEntities.Printers.Find(printerId);
        }

        public bool UpdatePrinter(Printer printer)
        {
            var oldPrinter = _tonerManagementEntities.Printers.Single(p=> p.printerId == printer.printerId);
            oldPrinter.customerId = printer.customerId;
            oldPrinter.cyanLowPercentage = printer.cyanLowPercentage;
            oldPrinter.isColour = printer.isColour;
            oldPrinter.keyingLowPercentage = printer.keyingLowPercentage;
            oldPrinter.magentaLowPercentage = printer.magentaLowPercentage;
            oldPrinter.printerName = printer.printerName;
            oldPrinter.yellowLowPercentage = printer.yellowLowPercentage;
            oldPrinter.stockLocationId = printer.stockLocationId;
            var updated = _tonerManagementEntities.SaveChanges();
            return updated > 0;

        }
    }
}