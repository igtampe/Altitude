using Igtampe.Altitude.Common;
using Igtampe.ChopoAuth;
using Igtampe.ChopoImageHandling;
using Igtampe.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace Igtampe.Altitude.Data {

    /// <summary>The Altitude Context</summary>
    public class AltitudeContext : PostgresContext, UserContext, ImageContext {

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

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

        /// <summary>Gets the trips of a given User</summary>
        /// <param name="Username">Username</param>
        /// <returns></returns>
        public IQueryable<Trip> UserTrips(string Username) => Trip
            .Include(A=>A.Days.OrderBy(A=>A.Index)).ThenInclude(A=>A.Events.OrderBy(A=>A.Index))
            .Where(A => A.Owner != null && A.Owner.Username == Username);

    }
}