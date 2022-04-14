namespace Igtampe.Altitude.API.Requests {
    /// <summary>Request to edit details of a Day</summary>
    public class DayRequest : Nameable, Describable, Depictable {

        /// <summary>Name of this day</summary>
        public string Name { get; set; } = "";

        /// <summary>Description of this day</summary>
        public string Description { get; set; } = "";

        /// <summary>Image of this day</summary>
        public string ImageURL { get; set; } = "";

        /// <summary>Color associated to this event</summary>
        public string Color { get; set; } = "";

        /// <summary>Icon associated to this event</summary>
        public int Icon { get; set; } = 0;

    }
}
