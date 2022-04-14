namespace Igtampe.Altitude.Common {

    /// <summary>A Day that's part of a trip</summary>
    public class Day : AutomaticallyGeneratableIdentifiable, Nameable, Describable, Depictable {

        /// <summary>Name of this day</summary>
        public string Name { get; set; } = "";

        /// <summary>Description of this day</summary>
        public string Description { get; set; } = "";

        /// <summary>Image of this day</summary>
        public string ImageURL { get; set; } = "";

        /// <summary>Trip this Day is tied to</summary>
        public Trip? Trip { get; set; }

        /// <summary>Index of this item</summary>
        public int Index { get; set; } = -1;

        /// <summary>List of events that occur this day</summary>
        public List<Event> Events { get; set; } = new();

        public List<Warning> Validate() {

            List<Warning> Warns = new();
            
            var TimedEvents = Events.Where(A=>A.Time != null).ToList();

            //Ensure everything is in order
            for (int i = 1; i < TimedEvents.Count; i++) {

                if (TimedEvents[i - 1].Time > TimedEvents[i].Time) {
                    Warns.Add(new() { Level=Warning.WarningLevel.WARNING, Item=TimedEvents[i-1].Name, 
                        Message=$"{TimedEvents[i-1].Name} Starts after {TimedEvents[i].Name}"});
                }

                if (TimedEvents[i].ReminderTime != -1 &&
                    TimedEvents[i - 1].Time > TimedEvents[i].Time!.Value.AddMinutes(-TimedEvents[i].ReminderTime)) {
                    Warns.Add(new() { Level = Warning.WarningLevel.WARNING, Item = TimedEvents[i].Name,
                        Message = $"{TimedEvents[i - 1].Name} starts after {TimedEvents[i].Name}'s reminder time"
                    });
                }

            }

            for (int i = 0; i < Events.Count; i++) {
                if (Events[i].Index != i) {
                    Warns.Add(new() { Level = Warning.WarningLevel.ERROR, Item = Events[i].Name, 
                        Message = $"Index mismatch on Item {Events[i].Name} ({Events[i].Index}!={i})"
                    });
                }
            }

            return Warns;
        }

    }
}
