﻿using Bazaar.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.BL.Services
{
    public interface ICRUDService
    {
        public Task<Tdto?> GetByIdAsync<Tdto>(Guid id, params string[] includes);

        public Task<IEnumerable<Tdto>> GetAllAsync<Tdto>();

        public Task CreateAsync<Tdto>(Tdto dto);
        public Task UpdateAsync<Tdto>(Guid id, Tdto dto);
        public Task DeleteAsync<Tdto>(Guid id);
    }
}
