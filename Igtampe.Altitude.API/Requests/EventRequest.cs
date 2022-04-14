using System.ComponentModel.DataAnnotations;

namespace Igtampe.Altitude.API.Requests {
    /// <summary>Request to edit the details of an event</summary>
    public class EventRequest : Nameable, Describable, Depictable {

        /// <summary>Name of the event</summary>
        public string Name { get; set; } = "";

        /// <summary>Description of the event</summary>
        public string Description { get; set; } = "";

        /// <summary>Image of this Event</summary>
        public string ImageURL { get; set; } = "";

        /// <summary>Color associated to this event</summary>
        public string Color { get; set; } = "";

        /// <summary>Icon associated to this event</summary>
        public int Icon { get; set; } = 0;

        /// <summary>Hour this event takes place on</summary>
        [Range(-1, 23)]
        public int Hour { get; set; } = -1;

        /// <summary>Minute this event takes place on</summary>
        [Range(0, 59)]
        public int Minute { get; set; } = 0;

        /// <summary>Time at which a reminder for this </summary>
        public int ReminderTime { get; set; } = -1;

    }
}
