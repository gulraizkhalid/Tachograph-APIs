using DataAccess.Postgres.Repositories;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Postgres.UnitOfWorks
{
    public class UnitOfWorkPG : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWorkPG(AppDbContext context)
        {
            _context = context;
            DriverActivity = new DriverActivityRepository(_context);
        }
        
        public IDriverActivityRepository DriverActivity { get; private set; }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
