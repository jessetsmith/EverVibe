using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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

            public string ResolveAddressSync()
            {
            //string Location;
            //GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            //watcher.MovementThreshold = 1.0; // set to one meter
            //watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));

            //CivicAddressResolver resolver = new CivicAddressResolver();

            //if (watcher.Position.Location.IsUnknown == false)
            //{
            //    CivicAddress address = resolver.ResolveAddress(watcher.Position.Location);

            //    if (!address.IsUnknown)
            //    {
            //        Location = address.CountryRegion + address.City;
            //        return _location = Location;
            //    }
            //    else
            //    {
            //        Location = "Address unknown.";
            //        return _location = Location;
            //    }
            //}
            //else
            //{
            //    Location = "Address unknown.";
            //    return _location = Location;
            //}

            String UserIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(UserIP))
            {
                UserIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            string url = $"http://ip-api.com/json/{UserIP}?fields=status,message,country,countryCode,region,regionName,city,zip,lat,lon,timezone,isp,org,as,query";
            WebClient client = new WebClient();
            string jsonstring = client.DownloadString(url);
            dynamic dynObj = JsonConvert.DeserializeObject(jsonstring);
            var city = HttpContext.Current.Session["City"];
            city = dynObj.city;


            return city.ToString();
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        public bool CreateVibe(VibeCreate model, HttpPostedFileBase file)
            {
            model.Image = ConvertToBytes(file);

            var userInfoService = new UserInfoService(_userID);
            var getUser = userInfoService.GetUsersByUserId(_userID);
            var username = getUser.Username;

            var ctx = new ApplicationDbContext();
          

            var entity =
                new Vibe()
                {
                    Id = _userID,
                    Username = username,
                    Title = model.Title,
                    Location = model.Location,
                    Image = model.Image,
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
                                Image = e.Image,
                                Description = e.Description,
                                //Tags = e.Tags
                            });
                    return query.ToArray();

                }
            }

        public IList<VibeDetail> GetVibeDetails()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Vibes
                    //.Where(e => e.UserID == _userID)
                    .Select(
                        e =>
                        new VibeDetail
                        {
                            VibeID = e.VibeID,
                            Username = e.Username,
                            Title = e.Title,
                            Location = e.Location,
                            Image = e.Image,
                            Description = e.Description,
                            Comments = e.Comments

                            //Tags = e.Tags
                        });
                return query.ToArray();

            }

        }

        public IEnumerable<VibeListItem> GetVibesByUser(string username)
        {
            var userInfoService = new UserInfoService(_userID);
            var getUser = userInfoService.GetUsersByUserId(_userID);
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
                            Image = e.Image,
                            Description = e.Description,
                            //Tags = e.Tags
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
                            Username = entity.Username,
                            Title = entity.Title,
                            Location = entity.Location,
                            Image = entity.Image,
                            Description = entity.Description,
                            Tags = entity.Tags,
                            Comments = entity.Comments
                        };
                }

            }

        public VibeListItem GetVibesByUserID(string id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Vibes
                    .Single(e => e.Id == id);
                return
                    new VibeListItem
                    {
                        VibeID = entity.VibeID,
                        Username = entity.Username,
                        Title = entity.Title,
                        Location = entity.Location,
                        Image = entity.Image,
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
                        .Single(e => e.Id == _userID);

                    entity.Username = username;
                    entity.Title = model.Title;
                    entity.Location = model.Location;
                    entity.Description = model.Description;
                    entity.Tags = model.Tags;
                    entity.DateModified = DateTimeOffset.UtcNow;

                    return ctx.SaveChanges() == 1;
                }

            }

            public bool DeleteVibe(int vibeID)
            {
                var ctx = new ApplicationDbContext();

                using (ctx)
                {
                    var entity =
                        ctx
                        .Vibes
                        .Single(e => e.VibeID == vibeID && e.Id == _userID);

                    ctx.Vibes.Remove(entity);
                    return ctx.SaveChanges() == 1;
                }
            }
     }
}
