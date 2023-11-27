using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDriverActivityRepository DriverActivity { get; }
        int Complete();
    }
}
