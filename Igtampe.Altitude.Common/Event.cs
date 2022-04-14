using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Igtampe.Altitude.Common {

    /// <summary>An event that's part of a day</summary>
    public class Event : AutomaticallyGeneratableIdentifiable, Nameable, Describable, Depictable {

        /// <summary>Name of the event</summary>
        public string Name { get; set; } = "";

        /// <summary>Description of the event</summary>
        public string Description { get; set; } = "";

        /// <summary>Image of this Event</summary>
        public string ImageURL { get; set; } = "";

        /// <summary>Index of this event</summary>
        public int Index { get; set; } = 0;

        /// <summary>Hour this event takes place on</summary>
        [Range(-1,23)]
        public int Hour { get; set; } = -1;

        /// <summary>Minute this event takes place on</summary>
        [Range(0,59)]
        public int Minute { get; set; } = 0;
        
        /// <summary>Time at which a reminder for this </summary>
        public int ReminderTime { get; set; } = -1;

        /// <summary>Backend only used TimeOnly var</summary>
        [JsonIgnore]
        [NotMapped]
        public TimeOnly? Time => Hour == -1 ? null : new(Hour,Minute);

    }
}
