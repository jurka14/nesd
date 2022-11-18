﻿using AutoMapper;
using Bazaar.BL.Dtos.Ad;
using Bazaar.BL.Dtos.Reaction;
using Bazaar.BL.Dtos.User;
using Bazaar.BL.QueryObjects;
using Bazaar.BL.Services.CRUDServices;
using Bazaar.DAL.Models;
using Bazaar.Infrastructure.UnitOfWork;

namespace Bazaar.BL.Services.Ads
{
    public class AdService : CRUDService<Ad>, IAdService
    {
        private readonly AdQueryObject _adQueryObject;
        public AdService(IUnitOfWork unitOfWork, IMapper mapper, AdQueryObject adQueryObject) : base(unitOfWork, mapper)
        {
            _adQueryObject = adQueryObject;
        }

        public async Task SetAdAsInvalid(Guid id)
        {
            var ad = await GetByIdAsync<AdEditDto>(id);
            if (ad == null)
            {
                throw new ArgumentException();
            }
            ad.IsValid = false;
            await UpdateAsync<AdEditDto>(ad);
        }


        public async Task<IEnumerable<ReactionDto>> GetAdReactions(Guid id)
        {
            var ad = await GetByIdAsync<AdDto>(id, nameof(Ad.Reactions));
            if (ad == null)
            {
                throw new ArgumentException();
            }
            return ad.Reactions;
        }

        public async Task<IEnumerable<AdListDto>> ExecuteQueryAsync(AdFilterDto filterDto)
        {
            return await _adQueryObject.ExecuteQueryAsync(filterDto);
        }
    }
}
