﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Bazaar.DAL.Models;
using Bazaar.Infrastructure.UnitOfWork;

namespace Bazaar.BL.Services
{
    public class TagService : CRUDService<Tag>
    {
        public TagService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }
    }
}
