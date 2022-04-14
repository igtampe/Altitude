namespace Igtampe.Altitude.Common {

    /// <summary>A Day that's part of a trip</summary>
    public class Day : AutomaticallyGeneratableIdentifiable, Nameable, Describable, Depictable {

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

        /// <summary>Index of this item</summary>
        public int Index { get; set; } = -1;

        /// <summary>List of events that occur this day</summary>
        public List<Event> Events { get; set; } = new();

        /// <summary>Gets list of warnings for this day</summary>
        /// <returns></returns>
        public List<Warning> Validate() {

            List<Warning> Warns = new();

            var TimedEvents = Events.Where(A => A.Time != null).ToList();

            //Ensure everything is in order
            for (int i = 1; i < TimedEvents.Count; i++) {

                if (TimedEvents[i - 1].Time > TimedEvents[i].Time) {
                    Warns.Add(new() {
                        Level = Warning.WarningLevel.WARNING, Item = TimedEvents[i - 1].Name,
                        Message = $"{TimedEvents[i - 1].Name} Starts after {TimedEvents[i].Name}"
                    });
                }

                if (TimedEvents[i].ReminderTime != -1 &&
                    TimedEvents[i - 1].Time > TimedEvents[i].Time!.Value.AddMinutes(-TimedEvents[i].ReminderTime)) {
                    Warns.Add(new() {
                        Level = Warning.WarningLevel.WARNING, Item = TimedEvents[i].Name,
                        Message = $"{TimedEvents[i - 1].Name} starts after {TimedEvents[i].Name}'s reminder time"
                    });
                }
            }

            for (int i = 0; i < Events.Count; i++) {
                if (Events[i].Index != i) {
                    Warns.Add(new() {
                        Level = Warning.WarningLevel.ERROR, Item = Events[i].Name,
                        Message = $"Index mismatch on Item {Events[i].Name} ({Events[i].Index}!={i})"
                    });
                }
            }

            return Warns;
        }

        /// <summary>Adds a day at the end of the list</summary>
        /// <param name="E"></param>
        public void AddEvent(Event E) {
            E.Index = Events.Count;
            Events.Add(E);
        }

        /// <summary>Insert a day at a specific point</summary>
        /// <param name="E"></param>
        /// <param name="Index"></param>
        public void AddEvent(Event E, int Index) {
            Events.Insert(Index, E); //Insert
            RecalculateEventIndexes(Index);
        }

        /// <summary>Exchanges two days</summary>
        /// <param name="Index"></param>
        /// <param name="NewIndex"></param>
        public void MoveEvent(int Index, int NewIndex) {

            //Ensure both indexes exist:
            if (Index < 0 || Index >= Events.Count) throw new IndexOutOfRangeException("Original index does not exist");
            if (NewIndex < 0 || NewIndex >= Events.Count) throw new InvalidOperationException("New Index does not exist");

            //Mira we can do this with legitimately no additional vars (at least in this code)
            //Watch:

            Events[Index].Index = NewIndex;
            Events[NewIndex].Index = Index;

            Events = Events.OrderBy(A => A.Index).ToList();

            //Another victory for Linq

        }

        /// <summary>Moves a day up by one index</summary>
        /// <param name="Index"></param>
        public void MoveEventUp(int Index) => MoveEvent(Index, Index - 1);

        /// <summary>Moves a day down by one index</summary>
        /// <param name="Index"></param>
        public void MoveEventDown(int Index) => MoveEvent(Index, Index + 1);

        /// <summary>Removes a day</summary>
        /// <param name="Index"></param>
        public Event RemoveEvent(int Index) {
            var E = Events[Index];
            Events.RemoveAt(Index); //Insert
            RecalculateEventIndexes(Index);
            return E;
        }

        private void RecalculateEventIndexes(int? Start = null) {
            for (int i = Start ?? 0; i < Events.Count; i++) { Events[i].Index = i; } //Update indices
        }
    }
}
