using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Vibespace.DATA;
using VibeSpace.DATA;
using VibeSpace.MODELS;

namespace VibeSpace.Services
{
    public class UserInfoService
    {
        private readonly string _userID;
        public string _location;

        public UserInfoService(string userID)
        {
            _userID = userID;
        }

        public string ResolveAddressSync()
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
            else {
                Location = "Address unknown.";
                return _location = Location;
            }

        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        public bool CreateUserInfo(UserInfoCreate model, HttpPostedFileBase file)
        {
            model.ProfileImage = ConvertToBytes(file);
            var ctx = new ApplicationDbContext();
            //var user = ctx.Users.Find(_userID);
            //_user = user;

            var entity =
                new UserInfo()
                {
                    Id = _userID,
                    Name = model.Name,
                    Username = model.Username,
                    Bio = model.Bio,
                    Location = model.Location,
                    ProfileImage = model.ProfileImage,
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
                    .Where(e => e.Id == _userID)
                    .Select(
                        e =>
                        new UserInfoListItem
                        {
                            Name = e.Name,
                            Username = e.Username,
                            ProfileImage = e.ProfileImage,
                            Location = e.Location
                        });
                return query.ToList();
                    
            }
        }

        public UserInfoDetail GetUsersByID(string id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .UsersInfo
                    .Single(e => e.Id == id);
                return
                    new UserInfoDetail
                    {
                        Name = entity.Name,
                        Username = entity.Username,
                        Bio = entity.Bio,
                        Location = entity.Location,
                        ProfileImage = entity.ProfileImage,
                        Interests = entity.Interests,
                        Vibes = entity.Vibes
                    };
            }

        }

        public UserInfoDetail GetUsersByUserId(string id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .UsersInfo
                    .FirstOrDefault(e => e.Id == id);
                if(entity != null)
                {
                    return
                        new UserInfoDetail
                        {
                            UserID = entity.Id,
                            Name = entity.Name,
                            Username = entity.Username,
                            Bio = entity.Bio,
                            Location = entity.Location,
                            ProfileImage = entity.ProfileImage,
                            Interests = entity.Interests,
                            Vibes = entity.Vibes
                        };
                }
                else
                {
                    return null;
                }
            }

        }

        public UserInfoEdit GetUsersByUserIdEdit(string id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                id = _userID;
                var entity =
                    ctx
                    .UsersInfo
                    .Single(e => e.Id == id);
                return
                    new UserInfoEdit
                    {
                        Name = entity.Name,
                        Username = entity.Username,
                        Bio = entity.Bio,
                        Location = entity.Location,
                        ProfileImage = entity.ProfileImage,
                        Interests = entity.Interests
                    };
            }

        }

        public UserInfoDetail GetUsersByUsername(string username)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .UsersInfo
                    .Single(e => e.Username == username);
                return
                    new UserInfoDetail
                    {
                        Name = entity.Name,
                        Username = entity.Username,
                        Bio = entity.Bio,
                        Location = entity.Location,
                        ProfileImage = entity.ProfileImage,
                        Interests = entity.Interests
                    };
            }

        }

        public bool UpdateUserInfo(UserInfoEdit model, HttpPostedFileBase file)
        {
            model.ProfileImage = ConvertToBytes(file);
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx
                    .UsersInfo
                    .Single(e => e.Id == _userID);

                entity.Name = model.Name;
                entity.Username = model.Username;
                entity.Bio = model.Bio;
                entity.Location = model.Location;
                entity.ProfileImage = model.ProfileImage;
                entity.Interests = model.Interests;
                entity.DateModified = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }

        }

        public bool DeleteUserInfo()
        {
            var ctx = new ApplicationDbContext();

            using (ctx)
            {
                var entity =
                    ctx
                    .UsersInfo
                    .Single(e => e.Id == _userID);

                ctx.UsersInfo.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }

    }
}
