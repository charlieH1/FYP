using System.Collections.Generic;
using TonerManagement.Models;

namespace TonerManagement.Repository.Interface
{
    public interface IPrinterRepo
    {
        Printer GetPrinter(int printerId);
        List<Printer> GetPrintersFromCustomer(int customerId);
        bool UpdatePrinter(Printer printer);
    }
}