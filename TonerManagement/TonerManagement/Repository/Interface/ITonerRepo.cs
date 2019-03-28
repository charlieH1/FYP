using TonerManagement.Models;

namespace TonerManagement.Repository.Interface
{
    public interface ITonerRepo
    {
        Toner GetToner(int tonerId);
    }
}