using Igtampe.ChopoAuth;
using System.ComponentModel.DataAnnotations.Schema;

namespace Igtampe.Altitude.Common {
    
    /// <summary>A Trip of some sort to some place</summary>
    public class Trip : AutomaticallyGeneratableIdentifiable, Nameable, Describable, Depictable, Updateable, Ownable<User> {

        /// <summary>Name of this trip</summary>
        public string Name { get; set; } = "";

        /// <summary>Description of this trip</summary>
        public string Description { get; set; } = "";

        /// <summary>Image of this trip</summary>
        public string ImageURL { get; set; } = "";

        /// <summary>Date this trip was last updated</summary>
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>Date this trip was created</summary>
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        /// <summary>User who owns this trip</summary>
        public User? Owner { get; set; }

        /// <summary>Whether or not this trip is public or not</summary>
        public bool Public { get; set; } = false;

        /// <summary>The date this trip starts on</summary>
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        /// <summary>End Date of this trip (Calculated by adding the number of days to the Start Date)</summary>
        public DateTime EndDate => StartDate.AddDays(Days.Count);

        /// <summary>Days this trip contains</summary>
        public List<Day> Days { get; set; } = new();

        /// <summary>List of warnings for all days</summary>
        [NotMapped]
        public List<Warning> Warnings => Validate();

        private List<Warning> Validate() {
            List<Warning> Warns = new();
            foreach (var item in Days) { Warns.AddRange(item.Validate()); }
            return Warns;
        }

        /// <summary>Adds a day at the end of the list</summary>
        /// <param name="D"></param>
        public void AddDay(Day D) {
            D.Index = Days.Count;
            Days.Add(D);
        }

        /// <summary>Insert a day at a specific point</summary>
        /// <param name="D"></param>
        /// <param name="Index"></param>
        public void AddDay(Day D, int Index) {
            Days.Insert(Index, D); //Insert
            RecalculateDayIndexes(Index);
        }

        /// <summary>Exchanges two days</summary>
        /// <param name="Index"></param>
        /// <param name="NewIndex"></param>
        public void MoveDay(int Index, int NewIndex) {

            //Ensure both indexes exist:
            if(Index < 0 || Index >= Days.Count) throw new IndexOutOfRangeException("Original index does not exist");
            if (NewIndex < 0 || NewIndex >= Days.Count) throw new InvalidOperationException("New Index does not exist");

            //Mira we can do this with legitimately no additional vars (at least in this code)
            //Watch:

            Days[Index].Index = NewIndex;
            Days[NewIndex].Index = Index;

            Days = Days.OrderBy(A => A.Index).ToList();
        
        }

        /// <summary>Moves a day up by one index</summary>
        /// <param name="Index"></param>
        public void MoveDayUp(int Index) => MoveDay(Index, Index - 1);

        /// <summary>Moves a day down by one index</summary>
        /// <param name="Index"></param>
        public void MoveDayDown(int Index) => MoveDay(Index, Index + 1);

        /// <summary>Removes a day</summary>
        /// <param name="Index"></param>
        public void RemoveDay(int Index) {
            Days.RemoveAt(Index); //Insert
            RecalculateDayIndexes(Index);
        }

        /// <summary>Moves an event to another day</summary>
        /// <param name="DayIndex"></param>
        /// <param name="EventIndex"></param>
        /// <param name="OtherDayIndex"></param>
        public void MoveEventToOtherDay(int DayIndex, int EventIndex, int OtherDayIndex) 
            => Days[OtherDayIndex].AddEvent(Days[DayIndex].RemoveEvent(EventIndex));

        private void RecalculateDayIndexes(int? Start = null) {
            for (int i = Start ?? 0; i < Days.Count; i++) { Days[i].Index = i; } //Update indices
        }
    }
}