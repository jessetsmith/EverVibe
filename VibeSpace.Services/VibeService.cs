using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeSpace.DATA;

namespace VibeSpace.Services
{
    class VibeService
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
            public bool CreateVibe( model)
            {
                var ctx = new ApplicationDbContext();
                var user = ctx.Users.Find(_userID);
                _user = user;

                var entity =
                    new UserInfo()
                    {
                        Name = model.Name,
                        Username = model.Username,
                        Bio = model.Bio,
                        Location = _location,
                        Interests = model.Interests
                    };
                using (ctx)
                {
                    ctx.UsersInfo.Add(entity);

                    return ctx.SaveChanges() == 1;
                }
            }

            public IEnumerable<UserInfoListItem> GetUsers()
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var query =
                        ctx
                        .UsersInfo
                        .Where(e => e.UserID == _userID)
                        .Select(
                            e =>
                            new UserInfoListItem
                            {
                                Name = e.Name,
                                Username = e.Username,
                                Location = e.Location
                            });
                    return query.ToArray();

                }
            }

            public UserInfoDetail GetUsersByID(string id)
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var entity =
                        ctx
                        .UsersInfo
                        .Single(e => e.UserID == id);
                    return
                        new UserInfoDetail
                        {
                            Name = entity.Name,
                            Username = entity.Username,
                            Bio = entity.Bio,
                            Location = entity.Location,
                            Interests = entity.Interests
                        };
                }

            }

            public bool UpdateUserInfo(UserInfoEdit model, int userID)
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var entity = ctx
                        .UsersInfo
                        .Single(e => e.UserID == _userID);

                    entity.Name = model.Name;
                    entity.Username = model.Username;
                    entity.Bio = model.Bio;
                    entity.Location = model.Location;
                    entity.Interests = model.Interests;
                    entity.DateModified = DateTimeOffset.UtcNow;

                    return ctx.SaveChanges() == 1;
                }

            }

            public bool DeleteUserInfo(int userID)
            {
                var ctx = new ApplicationDbContext();

                using (ctx)
                {
                    var entity =
                        ctx
                        .UsersInfo
                        .Single(e => e.UserID == _userID);

                    ctx.UsersInfo.Remove(entity);
                    return ctx.SaveChanges() == 1;
                }
            }
     }
}
