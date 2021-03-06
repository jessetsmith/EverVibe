﻿using System;
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
    public class CommentService
    {
        private readonly string _userID;
        private ApplicationUser _user;
        public string _location;

        public CommentService(string userID)
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
        public bool CreateComment(CommentCreate model, int id)
        {
            var userInfoService = new UserInfoService(_userID);
            var getUser = userInfoService.GetUsersByID(_userID);
            var username = getUser.Username;

            var vibeService = new VibeService(_userID, username);
            var getVibeID = vibeService.GetVibeDetailsByVibeId(id);
            
            id = getVibeID.VibeID;

            var ctx = new ApplicationDbContext();
            var user = ctx.Users.Find(_userID);
            _user = user;

            var entity =
                new CommentsAndReactions()
                {
                    Id = _userID,
                    VibeID = id,
                    Username = username,
                    CommentText = model.CommentText,
                    DateCreated = DateTimeOffset.UtcNow
                };
            using (ctx)
            {
                ctx.Comments_Reactions.Add(entity);

                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<CommentListItem> GetComments()
        {

            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Comments_Reactions
                    .Select(
                        e =>
                        new CommentListItem
                        {
                            // TODO: Change where you get username value from
                            //Username = e.Username,
                            DateCreated = e.DateCreated
                        });
                return query.ToArray();

            }
        }

        public IEnumerable<CommentListItem> GetCommentsByVibeID(int? id)
        {

            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Comments_Reactions
                    .Where(e => e.VibeID == id)
                    .Select(
                        e =>
                        new CommentListItem
                        {
                            CommentID = e.CommentID,
                            UserID = e.Id,
                            Username = e.Username,
                            CommentText = e.CommentText,
                            DateCreated = e.DateCreated
                        });
                return query.ToArray();

            }
        }

        public CommentDetail GetCommentsByID(int? id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Comments_Reactions
                    .Single(e => e.CommentID == id);
                return
                    new CommentDetail
                    {
                        Username = entity.Username,
                        CommentText = entity.CommentText,
                        DateCreated = entity.DateCreated
                    };
            }

        }

        public CommentEdit GetCommentsByIdEdit(int? id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Comments_Reactions
                    .Single(e => e.CommentID == id);
                return
                    new CommentEdit
                    {
                        CommentText = entity.CommentText
                        
                    };
            }

        }

        public bool UpdateComment(CommentEdit model, int id)
        {
            var userInfoService = new UserInfoService(_userID);
            var getUser = userInfoService.GetUsersByID(_userID);
            var username = getUser.Username;

            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx
                    .Comments_Reactions
                    .Single(e => e.Id == _userID && e.CommentID == id);

                entity.CommentText = model.CommentText;
                entity.DateModified = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }

        }

        public bool DeleteComments(int commentID)
        {

            var ctx = new ApplicationDbContext();

            using (ctx)
            {
                var entity =
                    ctx
                    .Comments_Reactions
                    .Single(e => e.CommentID == commentID && e.Id == _userID);

                ctx.Comments_Reactions.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
