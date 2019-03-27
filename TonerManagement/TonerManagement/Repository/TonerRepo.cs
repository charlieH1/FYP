using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TonerManagement.Models;
using TonerManagement.Repository.Interface;

namespace TonerManagement.Repository
{
    public class TonerRepo : ITonerRepo
    {
        private readonly TonerManagementEntities _db;

        public TonerRepo(DbContext db)
        {
            _db = (TonerManagementEntities) db;
        }

        public Toner GetToner(int tonerId)
        {
            return _db.Toners.Find(tonerId);
        }

    }
}