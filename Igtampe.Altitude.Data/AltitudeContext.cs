using Igtampe.Altitude.Common;
using Igtampe.ChopoAuth;
using Igtampe.ChopoImageHandling;
using Igtampe.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace Igtampe.Altitude.Data {

    /// <summary>The Altitude Context</summary>
    public class AltitudeContext : PostgresContext, UserContext, ImageContext {

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>Set of all TripShareData</summary>
        public DbSet<TripShareData> TripShareData { get; set;}

        /// <summary>Set of all Trips</summary>
        public DbSet<Trip> Trip { get; set; }

        /// <summary>Set of all Days</summary>
        public DbSet<Day> Day { get; set; }

        /// <summary>Set of all Events</summary>
        public DbSet<Event> Event { get; set; }

        /// <summary>Set of all users</summary>
        public DbSet<User> User { get; set; }

        /// <summary>Set of all images</summary>
        public DbSet<Image> Image { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>Set of Trips including Days and Events</summary>
        /// <returns></returns>
        public IQueryable<Trip> TripIncludes
            => Trip.Include(A => A.Days.OrderBy(A => A.Index)).ThenInclude(A => A.Events.OrderBy(A => A.Index));

        /// <summary>Set of Trips that are public</summary>
        /// <returns></returns>
        public IQueryable<Trip> PublicTrips => TripIncludes
            .Where(A => A.Public);

        /// <summary>Gets the trips of a given User</summary>
        /// <param name="Username">Username</param>
        /// <returns></returns>
        public IQueryable<Trip> UserTrips(string Username) => TripIncludes
            .Where(A => A.Owner != null && A.Owner.Username == Username);

        /// <summary>Gets all trips that have been shared with this user</summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public IQueryable<Trip> UserSharedTrips(string Username) => TripShareData
            .Where(A=>A.User != null && A.User.Username == Username && A.Trip!=null)
            .Include(A => A.Trip)
            .ThenInclude(A => A!.Days.OrderBy(A => A.Index))
            .ThenInclude(A => A!.Events.OrderBy(A => A.Index))
            .Select(A=>A.Trip)!;

        /// <summary>Gets an IQueryable with a specific trip</summary>
        /// <param name="Username"></param>
        /// <param name="ID"></param>
        /// <returns>A Tuple with a Trip, and a boolean on whether or not this user can edit it</returns>
        public async Task<(Trip?,bool)> GetTrip(string Username, Guid ID) {

            //Let's find the trip
            var AllPossibleTrips = PublicTrips.Union(UserTrips(Username)).Union(UserSharedTrips(Username));
            return new(
                await AllPossibleTrips.Where(A => A.ID == ID).FirstOrDefaultAsync(), 
                await UserTrips(Username).AnyAsync(A=>A.ID==ID)
            );
            
        }
    }
}