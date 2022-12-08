﻿using Bazaar.BL.Dtos.AdTag;
using Bazaar.BL.Dtos.Image;
using Bazaar.BL.Dtos.Tag;

namespace Bazaar.BL.Dtos.Ad
{
    public class AdListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public bool IsPremium { get; set; }

        public bool IsValid { get; set; }

        public bool IsOffer { get; set; }

        public int Price { get; set; }

        public UserDto Creator { get; set; }
        public ICollection<ImageDto> Images { get; set; }
        public ICollection<AdTagDto> AdTags { get; set; }
    }
}
