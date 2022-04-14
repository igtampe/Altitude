using Igtampe.ChopoAuth;

namespace Igtampe.Altitude.Common {
    /// <summary>Ownership data for a trip</summary>
    public class TripShareData : AutomaticallyGeneratableIdentifiable {

        /// <summary>Trip that's been shared</summary>
        public Trip? Trip { get; set; }

        /// <summary>User the trip was shared with</summary>
        public User? User { get; set; }
    }
}
