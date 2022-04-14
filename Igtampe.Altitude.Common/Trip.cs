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
    }
}