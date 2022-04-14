namespace Igtampe.Altitude.API.Requests {
    /// <summary>Request to edit details of a trip</summary>
    public class TripRequest : Nameable, Describable, Depictable {

        /// <summary>Name of this trip</summary>
        public string Name { get; set; } = "";

        /// <summary>Description of this trip</summary>
        public string Description { get; set; } = "";

        /// <summary>Image of this trip</summary>
        public string ImageURL { get; set; } = "";

        /// <summary>Whether or not this trip is public or not</summary>
        public bool Public { get; set; } = false;

        /// <summary>The date this trip starts on</summary>
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

    }
}
