using Igtampe.Altitude.Data;
using Igtampe.ChopoSessionManager;
using Igtampe.Controllers;

namespace Igtampe.Altitude.API.Controllers {

    /// <summary>A Controller for Altitude Users</summary>
    public class AltitudeUserController : UserController<AltitudeContext> {

        /// <summary>Creates an Altitude User Controller</summary>
        /// <param name="Context"></param>
        public AltitudeUserController(AltitudeContext Context) : base(Context, SessionManager.Manager) { }
    }
}
