using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vibespace.DATA;
using VibeSpace.DATA;
using VibeSpace.MODELS;

namespace VibeSpace.Services
{
    public class VibeService
    {
            private readonly string _userID;
            private ApplicationUser _user;
            public string _location;

            public VibeService(string userID)
            {
                _userID = userID;
            }

            string ResolveAddressSync()
            {
                string Location;
                GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                watcher.MovementThreshold = 1.0; // set to one meter
                watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));

                CivicAddressResolver resolver = new CivicAddressResolver();

                if (watcher.Position.Location.IsUnknown == false)
                {
                    CivicAddress address = resolver.ResolveAddress(watcher.Position.Location);

                    if (!address.IsUnknown)
                    {
                        Location = $"Country:{address.CountryRegion}, City: {address.City}";
                        return _location = Location;
                    }
                    else
                    {
                        Location = "Address unknown.";
                        return _location = Location;
                    }
                }
                else
                {
                    Location = "Address unknown.";
                    return _location = Location;
                }

            }
            public bool CreateVibe(VibeCreate model)
            {
            var userInfoService = new UserInfoService(_userID);
            var getUser = userInfoService.GetUsersByID(_userID);
            var username = getUser.Username;

                var ctx = new ApplicationDbContext();
                var user = ctx.Users.Find(_userID);
                _user = user;

            var entity =
                new Vibe()
                {
                    UserID = _userID,
                    Username = username,
                    Title = model.Title,
                    Location = _location,
                    Description = model.Description,
                    Tags = model.Tags,
                    Private = model.Private,
                    DateCreated = DateTimeOffset.UtcNow
                    };
                using (ctx)
                {
                    ctx.Vibes.Add(entity);

                    return ctx.SaveChanges() == 1;
                }
            }

            public IEnumerable<VibeListItem> GetVibes()
            {
         
            using (var ctx = new ApplicationDbContext())
                {
                    var query =
                        ctx
                        .Vibes
                        //.Where(e => e.UserID == _userID)
                        .Select(
                            e =>
                            new VibeListItem
                            {
                                VibeID = e.VibeID,
                                Username = e.Username,
                                Title = e.Title,
                                Location = e.Location,
                                Description = e.Description,
                                Tags = e.Tags
                            });
                    return query.ToArray();

                }
            }

        public IEnumerable<VibeListItem> GetVibesByUser(string username)
        {
            var userInfoService = new UserInfoService(_userID);
            var getUser = userInfoService.GetUsersByUsername(username);
            var _username = getUser.Username;

            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Vibes
                    .Where(e => e.Username == _username)
                    .Select(
                        e =>
                        new VibeListItem
                        {
                            VibeID = e.VibeID,
                            Username = e.Username,
                            Title = e.Title,
                            Location = e.Location,
                            Description = e.Description,
                            Tags = e.Tags
                        });
                return query.ToArray();

            }
        }

        public VibeDetail GetVibesByID(int? id)
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var entity =
                        ctx
                        .Vibes
                        .Single(e => e.VibeID == id);
                    return
                        new VibeDetail
                        {
                            VibeID = entity.VibeID,
                            Title = entity.Title,
                            Location = entity.Location,
                            Description = entity.Description,
                            Tags = entity.Tags,
                            Comments = entity.Comments
                        };
                }

            }

                

            public bool UpdateVibe(VibeEdit model)
            {
            var userInfoService = new UserInfoService(_userID);
            var getUser = userInfoService.GetUsersByID(_userID);
            var username = getUser.Username;

            using (var ctx = new ApplicationDbContext())
                {
                    var entity = ctx
                        .Vibes
                        .Single(e => e.UserID == _userID);

                    entity.Username = username;
                    entity.Title = model.Title;
                    entity.Location = model.Location;
                    entity.Description = model.Description;
                    entity.Tags = model.Tags;
                    entity.DateModified = DateTimeOffset.UtcNow;

                    return ctx.SaveChanges() == 1;
                }

            }

            public bool DeleteVibe()
            {
                var ctx = new ApplicationDbContext();

                using (ctx)
                {
                    var entity =
                        ctx
                        .Vibes
                        .Single(e => e.UserID == _userID);

                    ctx.Vibes.Remove(entity);
                    return ctx.SaveChanges() == 1;
                }
            }
     }
}
