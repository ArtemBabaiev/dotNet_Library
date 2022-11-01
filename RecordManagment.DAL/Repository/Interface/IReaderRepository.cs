﻿using RecordManagment.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.DAL.Repository.Interface
{
    public interface IReaderRepository : IGenericRepository<Reader>
    {
        Task<IEnumerable<Reader>> GetInactiveAllInactiveUsersInPeriod(DateTime lowerDate, DateTime upperDate);
    }
}
