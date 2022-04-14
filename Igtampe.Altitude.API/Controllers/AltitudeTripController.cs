using Igtampe.Altitude.API.Requests;
using Igtampe.Altitude.Common;
using Igtampe.Altitude.Data;
using Igtampe.ChopoAuth;
using Igtampe.ChopoSessionManager;
using Igtampe.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Igtampe.Altitude.API.Controllers {
    
    /// <summary>Controller for Trips in Altitude</summary>
    [ApiController]
    [Route("API/Trips")]
    public class AltitudeTripController : ErrorResultControllerBase {

        #region Props and Constructor

        private readonly AltitudeContext DB;
        private readonly ISessionManager Manager = SessionManager.Manager;

        /// <summary>Creates an Altitude Trip Controller</summary>
        /// <param name="Context"></param>
        public AltitudeTripController(AltitudeContext Context) => DB = Context;

        #endregion

        #region Get Trips

        //Get/Search Trips

        /// <summary>Gets a list of all trips owned by logged in user</summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        [HttpGet("Owned")]
        public async Task<IActionResult> GetOwned([FromHeader] Guid? SessionID) {
            //Check the session:
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            return S is null ? InvalidSession() : Ok(await DB.UserTrips(S.Username).OrderByDescending(A=>A.DateUpdated).ToListAsync());
        }

        /// <summary>Get a list of all trips shared by the logged in user</summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        [HttpGet("Shared")]
        public async Task<IActionResult> GetShared([FromHeader] Guid? SessionID) {
            //Check the session:
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            return S is null ? InvalidSession() : Ok(await DB.UserSharedTrips(S.Username).OrderByDescending(A => A.DateUpdated).ToListAsync());
        }

        #endregion

        #region Trip ShareData

        //Get TripShareData
        /// <summary>Get ShareData for for this trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("{ID}/ShareData")]
        public async Task<IActionResult> GetShareData([FromHeader] Guid? SessionID, [FromRoute] Guid ID) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            var TripGetResult = await DB.GetTrip(S.Username, ID);
            return TripGetResult.Item1 is null || !TripGetResult.Item2 
                ? NotFoundItem("TripShareData",ID) 
                : Ok(TripGetResult.Item1.ShareData);
        }

        //Add TripShareData
        /// <summary>Add a user to the ShareData of this trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost("{ID}/ShareData")]
        public async Task<IActionResult> AddShareData([FromHeader] Guid? SessionID, [FromRoute] Guid ID, [FromBody] ShareDataRequest Request) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            var TripGetResult = await DB.GetTrip(S.Username, ID);
            if (TripGetResult.Item1 is null || !TripGetResult.Item2) return NotFoundItem("TripShareData", ID);

            var User = await DB.User.FindAsync(Request.Username);
            if(User is null) { return NotFoundItem("User", Request.Username); }

            TripShareData ShareData = new() { Trip=TripGetResult.Item1, User=User };

            DB.Add(ShareData);
            
            await DB.SaveChangesAsync();
            return await GetShareData(SessionID,ID);
        }

        //Remove TripShareData
        /// <summary>Remove a user from the ShareData of this trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpDelete("{ID}/ShareData")]
        public async Task<IActionResult> RemoveShareDAta([FromHeader] Guid? SessionID, [FromRoute] Guid ID, [FromBody] ShareDataRequest Request) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            DB.Remove(
                DB.TripShareData
                .Where(A => A.Trip != null && 
                A.Trip.ID == ID && 
                A.User != null && 
                A.User.Username == Request.Username));

            await DB.SaveChangesAsync();
            return await GetShareData(SessionID, ID);
        }

        #endregion

        #region Trips

        //Get Trip
        /// <summary>Gets a specified trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetTrip([FromHeader] Guid? SessionID, [FromRoute] Guid ID) {
            //Check the session:
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            Trip? T = (await DB.GetTrip(S.Username, ID)).Item1;
            return T is null
                ? NotFoundItem("Trip", ID)
                : Ok(T);

        }

        //Create Trip
        /// <summary>Creates a trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateTrip([FromHeader] Guid? SessionID, [FromBody] TripRequest Request) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            User? U = await DB.User.FindAsync(S.Username);
            if (U is null) { return InvalidSession(); }

            Trip T = new() {
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                Description = Request.Description,
                ImageURL = Request.ImageURL,
                Name = Request.Name,
                Owner = U,
                Public = Request.Public,
                StartDate = Request.StartDate,
            };

            DB.Add(T);
            await DB.SaveChangesAsync();

            return Ok(T);
        }

        //Edit Trip Details
        /// <summary>Edits a trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPut("{ID}")]
        public async Task<IActionResult> EditTrip([FromHeader] Guid? SessionID, [FromRoute] Guid ID, [FromBody] TripRequest Request) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            User? U = await DB.User.FindAsync(S.Username);
            if (U is null) { return InvalidSession(); }

            (Trip? T, bool CanEdit) = await DB.GetTrip(S.Username, ID);
            if(T is null || !CanEdit) { return NotFound("Cannot find or edit this trip"); }

            T.Description = Request.Description;
            T.ImageURL = Request.ImageURL;
            T.Name = Request.Name;
            T.Owner = U;
            T.Public = Request.Public;
            T.StartDate = Request.StartDate;

            T.DateUpdated = DateTime.UtcNow;
            DB.Update(T);
            await DB.SaveChangesAsync();

            return Ok(T);
        }

        //

        //Delete Trip
        /// <summary>Deletes a trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteTrip([FromHeader] Guid? SessionID, [FromRoute] Guid ID) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            (Trip? T, bool CanEdit) = await DB.GetTrip(S.Username, ID);
            if (T is null || !CanEdit) { return NotFound("Cannot find or edit this trip"); }

            DB.Remove(T);
            await DB.SaveChangesAsync();

            return Ok(T);
        }

        #endregion Trips

        #region Days

        //Add Day
        /// <summary>Add a day to a trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost("{ID}/Days")]
        public async Task<IActionResult> AddDay([FromHeader] Guid? SessionID, [FromRoute] Guid ID, [FromBody] DayRequest Request) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            (Trip? T, bool CanEdit) = await DB.GetTrip(S.Username, ID);
            if (T is null || !CanEdit) { return NotFound("Cannot find or edit this trip"); }

            T.Days.Add(new() {
                Name = Request.Name,
                Description = Request.Description,
                ImageURL = Request.ImageURL,
                Color = Request.Color,
                Icon = Request.Icon
            });

            T.DateUpdated = DateTime.UtcNow;
            DB.Update(T);
            await DB.SaveChangesAsync();

            return Ok(T);
        }

        //Edit Day Details
        /// <summary>Edit a day in a trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Day"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPut("{ID}/Days/{Day}")]
        public async Task<IActionResult> EditDay([FromHeader] Guid? SessionID, [FromRoute] Guid ID, [FromRoute] int Day, [FromBody] DayRequest Request) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            (Trip? T, bool CanEdit) = await DB.GetTrip(S.Username, ID);
            if (T is null || !CanEdit) { return NotFound("Cannot find or edit this trip"); }
            if (Day < 0 || Day >= T.Days.Count) { return BadRequest("Day to edit does not exist"); }

            T.Days[Day].Name=Request.Name;
            T.Days[Day].Description=Request.Description;
            T.Days[Day].ImageURL=Request.ImageURL;
            T.Days[Day].Color = Request.Color;
            T.Days[Day].Icon = Request.Icon;

            T.DateUpdated = DateTime.UtcNow;
            DB.Update(T);
            await DB.SaveChangesAsync();

            return Ok();
        }

        //Move Days
        /// <summary>Move a day up one in the list of days in a trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Day"></param>
        /// <returns></returns>
        [HttpPut("{ID}/Days/{Day}/MoveUp")]
        public async Task<IActionResult> MoveDayUp([FromHeader] Guid? SessionID, [FromRoute] Guid ID, [FromRoute] int Day) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            (Trip? T, bool CanEdit) = await DB.GetTrip(S.Username, ID);
            if (T is null || !CanEdit) { return NotFound("Cannot find or edit this trip"); }
            if (Day < 0 || Day >= T.Days.Count) { return BadRequest("Day to edit does not exist"); }

            try { T.MoveDayUp(Day); } 
            catch (IndexOutOfRangeException) { return BadRequest("Could not move item to requested position"); }

            T.DateUpdated = DateTime.UtcNow;
            DB.Update(T);
            await DB.SaveChangesAsync();

            return Ok();
        }

        /// <summary>Move a day down one in this list of days in a trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Day"></param>
        /// <returns></returns>
        [HttpPut("{ID}/Days/{Day}/MoveDown")]
        public async Task<IActionResult> MoveDayDown([FromHeader] Guid? SessionID, [FromRoute] Guid ID, [FromRoute] int Day) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            (Trip? T, bool CanEdit) = await DB.GetTrip(S.Username, ID);
            if (T is null || !CanEdit) { return NotFound("Cannot find or edit this trip"); }
            if (Day < 0 || Day >= T.Days.Count) { return BadRequest("Day to edit does not exist"); }

            try { T.MoveDayDown(Day); } 
            catch (IndexOutOfRangeException) { return BadRequest("Could not move item to requested position"); }

            T.DateUpdated = DateTime.UtcNow;
            DB.Update(T);
            await DB.SaveChangesAsync();

            return Ok();
        }

        /// <summary>Moves a day to a new index</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Day"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [HttpPut("{ID}/Days/{Day}/MoveTo/{index}")]
        public async Task<IActionResult> MoveDay([FromHeader] Guid? SessionID, [FromRoute] Guid ID, 
            [FromRoute] int Day, [FromRoute] int index) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            (Trip? T, bool CanEdit) = await DB.GetTrip(S.Username, ID);
            if (T is null || !CanEdit) { return NotFound("Cannot find or edit this trip"); }
            if (Day < 0 || Day >= T.Days.Count) { return BadRequest("Day to edit does not exist"); }

            try { T.MoveDay(Day,index); } 
            catch (IndexOutOfRangeException) { return BadRequest("Could not move item to requested position"); }

            T.DateUpdated = DateTime.UtcNow;
            DB.Update(T);
            await DB.SaveChangesAsync();

            return Ok();
        }

        //Remove Day
        /// <summary>Removes a day from a trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Day"></param>
        /// <returns></returns>
        [HttpDelete("{ID}/Days/{Day}")]
        public async Task<IActionResult> RemoveDay([FromHeader] Guid? SessionID, [FromRoute] Guid ID, [FromRoute] int Day) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            (Trip? T, bool CanEdit) = await DB.GetTrip(S.Username, ID);
            if (T is null || !CanEdit) { return NotFound("Cannot find or edit this trip"); }
            if (Day < 0 || Day >= T.Days.Count) { return BadRequest("Day to edit does not exist"); }

            T.RemoveDay(Day);

            T.DateUpdated = DateTime.UtcNow;
            DB.Update(T);
            await DB.SaveChangesAsync();

            return Ok();
        }

        #endregion Days

        #region Events

        //Add Event
        /// <summary>Add an Event to a Day in a Trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Day"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost("{ID}/Days/{Day}/Events")]
        public async Task<IActionResult> AddEvent([FromHeader] Guid? SessionID, [FromRoute] Guid ID, 
            [FromRoute] int Day, [FromBody] EventRequest Request) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            (Trip? T, bool CanEdit) = await DB.GetTrip(S.Username, ID);
            if (T is null || !CanEdit) { return NotFound("Cannot find or edit this trip"); }
            if (Day < 0 || Day >= T.Days.Count) { return BadRequest("Day to edit does not exist"); }

            Event E = new() {
                Name= Request.Name,
                Description = Request.Description,
                ImageURL = Request.ImageURL,
                Color = Request.Color,
                Icon = Request.Icon,
                Hour = Request.Hour,
                Minute = Request.Minute,
                ReminderTime = Request.ReminderTime,
            };

            T.DateUpdated = DateTime.UtcNow;
            DB.Update(T);
            await DB.SaveChangesAsync();

            return Ok();
        }

        //Edit Event Details
        /// <summary>Edit an event in a day in a trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Day"></param>
        /// <param name="Event"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPut("{ID}/Days/{Day}/Events/{Event}")]
        public async Task<IActionResult> EditEvent([FromHeader] Guid? SessionID, [FromRoute] Guid ID,
            [FromRoute] int Day, [FromRoute] int Event, [FromBody] EventRequest Request) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            (Trip? T, bool CanEdit) = await DB.GetTrip(S.Username, ID);
            if (T is null || !CanEdit) { return NotFound("Cannot find or edit this trip"); }
            if (Day < 0 || Day >= T.Days.Count) { return BadRequest("Day to edit does not exist"); }
            if (Event < 0 || Event >= T.Days[Day].Events.Count) { return BadRequest("Event to edit does not exist"); }

            T.Days[Day].Events[Event].Name = Request.Name;
            T.Days[Day].Events[Event].Description = Request.Description;
            T.Days[Day].Events[Event].ImageURL = Request.ImageURL;
            T.Days[Day].Events[Event].Color = Request.Color;
            T.Days[Day].Events[Event].Icon = Request.Icon;
            T.Days[Day].Events[Event].Hour = Request.Hour;
            T.Days[Day].Events[Event].Minute = Request.Minute;
            T.Days[Day].Events[Event].ReminderTime = Request.ReminderTime;

            T.DateUpdated = DateTime.UtcNow;
            DB.Update(T);
            await DB.SaveChangesAsync();

            return Ok();
        }

        //Move Events
        /// <summary>Move an event up one in the list of events in a day in a trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Day"></param>
        /// <param name="Event"></param>
        /// <returns></returns>
        [HttpPut("{ID}/Days/{Day}/Events/{Event}/MoveUp")]
        public async Task<IActionResult> MoveEventUp([FromHeader] Guid? SessionID, [FromRoute] Guid ID,
            [FromRoute] int Day, [FromRoute] int Event) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            (Trip? T, bool CanEdit) = await DB.GetTrip(S.Username, ID);
            if (T is null || !CanEdit) { return NotFound("Cannot find or edit this trip"); }
            if (Day < 0 || Day >= T.Days.Count) { return BadRequest("Day to edit does not exist"); }
            if (Event < 0 || Event >= T.Days[Day].Events.Count) { return BadRequest("Event to edit does not exist"); }

            try { T.Days[Day].MoveEventUp(Event); } 
            catch (IndexOutOfRangeException) { return BadRequest("Could not move item to requested position"); }

            T.DateUpdated = DateTime.UtcNow;
            DB.Update(T);
            await DB.SaveChangesAsync();

            return Ok();
        }

        /// <summary>Move an event down one in the list of events in a day in a trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Day"></param>
        /// <param name="Event"></param>
        /// <returns></returns>
        [HttpPut("{ID}/Days/{Day}/Events/{Event}/MoveDown")]
        public async Task<IActionResult> MoveEventDown([FromHeader] Guid? SessionID, [FromRoute] Guid ID, 
            [FromRoute] int Day, [FromRoute] int Event) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            (Trip? T, bool CanEdit) = await DB.GetTrip(S.Username, ID);
            if (T is null || !CanEdit) { return NotFound("Cannot find or edit this trip"); }
            if (Day < 0 || Day >= T.Days.Count) { return BadRequest("Day to edit does not exist"); }
            if (Event < 0 || Event >= T.Days[Day].Events.Count) { return BadRequest("Event to edit does not exist"); }

            try { T.Days[Day].MoveEventDown(Event); } 
            catch (IndexOutOfRangeException) { return BadRequest("Could not move item to requested position"); }

            T.DateUpdated = DateTime.UtcNow;
            DB.Update(T);
            await DB.SaveChangesAsync();

            return Ok();
        }

        /// <summary>Move an event to a specified index in the list of events in a day in a trip</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Day"></param>
        /// <param name="Event"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        [HttpPut("{ID}/Days/{Day}/Events/{Event}/MoveTo/{Index}")]
        public async Task<IActionResult> MoveEvent([FromHeader] Guid? SessionID, [FromRoute] Guid ID, 
            [FromRoute] int Day, [FromRoute] int Event, [FromRoute] int Index) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            (Trip? T, bool CanEdit) = await DB.GetTrip(S.Username, ID);
            if (T is null || !CanEdit) { return NotFound("Cannot find or edit this trip"); }
            if (Day < 0 || Day >= T.Days.Count) { return BadRequest("Day to edit does not exist"); }
            if (Event < 0 || Event >= T.Days[Day].Events.Count) { return BadRequest("Event to edit does not exist"); }

            try { T.Days[Day].MoveEvent(Event,Index); } 
            catch (IndexOutOfRangeException) { return BadRequest("Could not move item to requested position"); }

            T.DateUpdated = DateTime.UtcNow;
            DB.Update(T);
            await DB.SaveChangesAsync();

            return Ok();
        }

        /// <summary>Move an event from one day to another day</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Day"></param>
        /// <param name="Event"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        [HttpPut("{ID}/Days/{Day}/Events/{Event}/MoveToDay/{Index}")]
        public async Task<IActionResult> MoveEventToOtherDay([FromHeader] Guid? SessionID, [FromRoute] Guid ID, 
            [FromRoute] int Day, [FromRoute] int Event, [FromRoute] int Index) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            (Trip? T, bool CanEdit) = await DB.GetTrip(S.Username, ID);
            if (T is null || !CanEdit) { return NotFound("Cannot find or edit this trip"); }
            if (Day < 0 || Day >= T.Days.Count) { return BadRequest("Day to edit does not exist"); }
            if (Index < 0 || Index >= T.Days.Count) { return BadRequest("Day to move to does not exist"); }
            if (Event < 0 || Event >= T.Days[Day].Events.Count) { return BadRequest("Event to edit does not exist"); }

            try { T.MoveEventToOtherDay(Day,Event,Index); } 
            catch (IndexOutOfRangeException) { return BadRequest("Could not move item to requested position"); }

            T.DateUpdated = DateTime.UtcNow;
            DB.Update(T);
            await DB.SaveChangesAsync();

            return Ok();
        }

        //Remove Event
        /// <summary>Remove a day </summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <param name="Day"></param>
        /// <param name="Event"></param>
        /// <returns></returns>
        [HttpDelete("{ID}/Days/{Day}/Events/{Event}")]
        public async Task<IActionResult> RemoveEvent([FromHeader] Guid? SessionID, [FromRoute] Guid ID, 
            [FromRoute] int Day, [FromRoute] int Event) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID));
            if (S is null) { return InvalidSession(); }

            (Trip? T, bool CanEdit) = await DB.GetTrip(S.Username, ID);
            if (T is null || !CanEdit) { return NotFound("Cannot find or edit this trip"); }
            if (Day < 0 || Day >= T.Days.Count) { return BadRequest("Day to edit does not exist"); }
            if (Event < 0 || Event >= T.Days[Day].Events.Count) { return BadRequest("Event to edit does not exist"); }

            T.Days[Day].RemoveEvent(Event);

            T.DateUpdated = DateTime.UtcNow;
            DB.Update(T);
            await DB.SaveChangesAsync();

            return Ok();
        }

        #endregion Events

    }
}
