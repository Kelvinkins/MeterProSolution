using MeterPro.DATA.DataContexts;
using MeterPro.DATA.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterPro.DATA.DAL
{
    public class UnitOfWork : IDisposable
    {
        public MeterProDataContext _context;
        public UnitOfWork(MeterProDataContext context)
        {
            _context = context;
        }
        private MeterProRepository<TimeData> timeDataRepository;



        public MeterProRepository<TimeData> TimeDataRepository
        {
            get
            {

                if (this.timeDataRepository == null)
                {
                    this.timeDataRepository = new MeterProRepository<TimeData>(_context);
                }
                return timeDataRepository;
            }
        }



        public async Task<int> CommitAsync()
        {
            return await _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
