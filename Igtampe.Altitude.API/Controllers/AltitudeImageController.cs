using Igtampe.Altitude.Data;
using Igtampe.ChopoSessionManager;
using Igtampe.Controllers;

namespace Igtampe.Altitude.API.Controllers {

    /// <summary>An Image Controller for Altitude</summary>
    public class AltitudeImageController : ImageController<AltitudeContext> {

        /// <summary>Creates an Altitude Image Controller</summary>
        /// <param name="Context"></param>
        public AltitudeImageController(AltitudeContext Context) : base(Context, SessionManager.Manager) { }
    }
}
