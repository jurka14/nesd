using AutoMapper;
using Bazaar.BL.Dtos.User;
using Bazaar.BL.Dtos.Ad;
using Bazaar.DAL.Data;
using Bazaar.DAL.Models;
using Bazaar.Infrastructure.EFCore.Query;
using Microsoft.EntityFrameworkCore;
using Bazaar.BL.QueryObjects.Ads;
using Bazaar.BL.QueryObjects.Users;
using Optional;
using Microsoft.Extensions.DependencyInjection;

namespace Bazzar.Bl.Tests
{
    public class QueryObjectTests
    {
        private DbContextOptions<BazaarDBContext> _options;


        public QueryObjectTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .AddEntityFrameworkProxies()
                .BuildServiceProvider();

            _options = new DbContextOptionsBuilder<BazaarDBContext>()
                .UseInMemoryDatabase(databaseName: $"test_db_{Guid.NewGuid()}")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            using var _bazaarDbContext = new BazaarDBContext(_options);

            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var adId1 = Guid.NewGuid();
            var adId2 = Guid.NewGuid();
            var adId3 = Guid.NewGuid();
            var adId4 = Guid.NewGuid();
            var adId5 = Guid.NewGuid();
            var tagId1 = Guid.NewGuid();
            var tagId2 = Guid.NewGuid();
            var tagId3 = Guid.NewGuid();
            var tagId4 = Guid.NewGuid();
            var tagId5 = Guid.NewGuid();
            _bazaarDbContext.User.Add
            (
                new User
                {
                    Id = userId1,
                    UserName = "TestUser1",
                    FirstName = "Ferko",
                    LastName = "Mrkvicka",
                    Email = "jozko@gmailol.com",
                    PhoneNumber = "0000000",
                    PasswordHash = "tajneheslo",
                    Banned = false

                }
             ); 
            _bazaarDbContext.User.Add
            (
                new User
                {
                    Id = userId2,
                    UserName = "ATestUser2",
                    FirstName = "AFerko",
                    LastName = "Priezviskovy",
                    Email = "ferko@gmailol.com",
                    PhoneNumber = "2020040444",
                    PasswordHash = "supertajneheslo",
                    Banned = false
                }
            ); ;

            var nepotrebneTag = new Tag
            {
                Id = tagId1,
                TagName = "Nepotrebne"
            };

            var pesTag = new Tag
            {
                Id = tagId2,
                TagName = "Pes"
            };

            var zvieraTag = new Tag
            {
                Id = tagId3,
                TagName = "Zviera"
            };

            var ostatneTag = new Tag
            {
                Id = tagId4,
                TagName = "Ostatne"
            };


            var svorkaTag = new Tag
            {
                Id = tagId5,
                TagName = "Svokra"
            };

            _bazaarDbContext.Tag.Add
            (
                ostatneTag
            );

            _bazaarDbContext.Tag.Add
            (
                pesTag
            );

            _bazaarDbContext.Tag.Add
            (
                nepotrebneTag
            );

            _bazaarDbContext.Tag.Add
            (
                zvieraTag
            );
            _bazaarDbContext.Ad.Add
            (
                new Ad
                {
                    Id = adId1,
                    Title = "Predam psa",
                    Description = "Je velmi zlata, zbavte ma jej, prosim",
                    IsOffer = true,
                    IsPremium = false,
                    IsValid = true,
                    Price = 100,
                    UserId = userId1,
                    Tags = new List<Tag>()
    
                    {
                        pesTag, zvieraTag, ostatneTag
                    }
                }
            );
            _bazaarDbContext.Ad.Add
            (
                new Ad
                {
                    Id = adId2,
                    Title = "Predam macku",
                    Description = "Je velmi zlata, mam ich moc vela, dalsiu nechcem!",
                    IsOffer = true,
                    IsPremium = true,
                    IsValid = true,
                    Price = 100,
                    UserId = userId1,
                    Tags = new List<Tag>()
                    {
                        zvieraTag
                    }
                }
            );
            _bazaarDbContext.Ad.Add
            (
                new Ad
                {
                    Id = adId3,
                    Title = "Predam strom",
                    Description = "je to velmi pekny strom!",
                    IsOffer = true,
                    IsPremium = false,
                    IsValid = true,
                    Price = 900,
                    UserId = userId1,
                    Tags = new List<Tag>()
                    {
                        ostatneTag
                    }
                }
            );
            _bazaarDbContext.Ad.Add
            (
                new Ad
                {
                    Id = adId4,
                    Title = "HRABLE NA PREDAJ, VOLAJ IHNED!!!",
                    Description = "Multifunkcne hrable na solarny pohon!",
                    IsOffer = true,
                    IsPremium = true,
                    IsValid = true,
                    Price = 111,
                    UserId = userId2,
                    Tags = new List<Tag>()
                    {
                        ostatneTag
                    }
                }
            );
            _bazaarDbContext.Ad.Add
            (
                new Ad
                {
                    Id = adId5,
                    Title = "Predam svoju svokru!",
                    Description = "Kto chce mat doma svokru..",
                    IsOffer = true,
                    IsPremium = false,
                    IsValid = true,
                    Price = 0,
                    UserId = userId2,
                    Tags = new List<Tag>()
                    {
                        svorkaTag
                    }
                }
            );
            _bazaarDbContext.SaveChanges();
        }

        [Fact]
        public async Task GetUserNameLike_ReturnsCorrectUserDtos()
        {
            using var _bazaarDbContext = new BazaarDBContext(_options);
            var query = new EFQuery<User>(_bazaarDbContext);

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<User, UserListDto>().ReverseMap();
            });

            var mapper = new Mapper(config);
            var userQueryObject = new UserQueryObject(mapper, query);
            var user_filter_dto = new UserFilterDto { LikeUserName = "TestUser".Some(), OderCriteria = "UserName".Some()};
            var result = await userQueryObject.ExecuteQueryAsync(user_filter_dto);

            Assert.Equal(2, result.Count());
            Assert.Equal("ATestUser2", result.First().UserName);
        }

        [Fact]
        public async Task GetUserByExactUsername_ReturnsCorrectUserDto()
        {
            using var _bazaarDbContext = new BazaarDBContext(_options);
            var query = new EFQuery<User>(_bazaarDbContext);

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<User, UserListDto>().ReverseMap();
            });

            var mapper = new Mapper(config);
            var userQueryObject = new UserQueryObject(mapper, query);
            var user_filter_dto = new UserFilterDto { ContainsUserName = "TestUser1".Some(), OderCriteria = "Username".Some() };
            var result = await userQueryObject.ExecuteQueryAsync(user_filter_dto);

            Assert.Equal(1, result.Count());
            Assert.Equal("TestUser1", result.First().UserName);
        }

        [Fact]
        public async Task GetAdByExactTitle_ReturnsCorrectAdDto()
        {
            using var _bazaarDbContext = new BazaarDBContext(_options);
            var query = new EFQuery<Ad>(_bazaarDbContext);

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Ad, AdListDto>().ReverseMap();
            });

            var mapper = new Mapper(config);
            var adQueryObject = new AdQueryObject(mapper, query);
            var ad_filter_dto = new AdFilterDto() { ContainsTitleName = "Predam svoju svokru!".Some() };
            var result = await adQueryObject.ExecuteQueryAsync(ad_filter_dto);

            Assert.Equal(1, result.Count());
            Assert.Equal("Predam svoju svokru!", result.First().Title);
        }

        [Fact]
        public async Task GetAdsByTags_ReturnsCorrectAdDtos()
        {
            using var _bazaarDbContext = new BazaarDBContext(_options);
            var query = new EFQuery<Ad>(_bazaarDbContext);

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Ad, AdListDto>().ReverseMap();
            });

            var mapper = new Mapper(config);
            var adQueryObject = new AdQueryObject(mapper, query);
            var tagNames = new List<string>() { "Pes", "Zviera", "Svokra" };
            var ad_filter_dto = new AdFilterDto() { TagNames = tagNames.Some(), OderCriteria = "Title".Some() };
            var result = await adQueryObject.ExecuteQueryAsync(ad_filter_dto);

            Assert.Equal(3, result.Count());
            Assert.Equal("Predam macku", result.First().Title);
        }
    }
}