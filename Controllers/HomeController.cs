using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EventFlow.Models;
using EventFlow.Repository;

namespace EventFlow.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IEventFlowRepository _eventFlowRepository;


    public HomeController(ILogger<HomeController> logger, IEventFlowRepository eventFlowRepository)
    {
        _logger = logger;
        _eventFlowRepository = eventFlowRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("/GetReservations/{userid}")]
    public async Task<IActionResult> GetReservation(int userid)
    {
        try
        {
            List<Reservation> reservation = await _eventFlowRepository.GetEventReservation(userid);
            return Ok(new { reservation });
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message, ex);
            return BadRequest(ex.Message);
        }
        
    }

    [HttpPost("/CreateReservation")]
    public async Task<IActionResult> CreateReservation([FromBody] Reservation reservation)
    {
        try
        {
            var newReservation = await _eventFlowRepository.AddReservation(reservation.ReservationTime, reservation.ReservationDate, reservation.Status, reservation.UserId, reservation.EventId);
            return Json(new { newReservation });
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message, ex);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("/CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        try
        {
            var newUser = await _eventFlowRepository.AddUser(user.Username, user.Email);
            return Json(new { newUser });
        } 
        catch (Exception ex) 
        {
            _logger.LogError(ex.Message, ex);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("/DeleteReservation/{reservationId}")]
    public async Task<IActionResult> DeleteReservation(int reservationId)
    {
        try
        {
            await _eventFlowRepository.DeleteReservation(reservationId);
            return Ok("Reservation deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("/UpdateReservation/{reservationId}")]
    public async Task<IActionResult> UpdateReservation(int reservationId, [FromBody] Reservation reservation)
    {
        try
        {
            await _eventFlowRepository.UpdateReservation(reservationId, reservation.ReservationTime, reservation.ReservationDate, reservation.Status);
            return Ok("Reservation updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("/GetAllEvents")]
    public async Task<IActionResult> GetAllEvents()
    {
        try
        {
            List<Event> events = await _eventFlowRepository.GetAllEvents();
            return Ok(new { events }); // Ensure the response has an 'events' property
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return BadRequest(ex.Message);
        }
    }

//sql5749035
//sql5749035
//3306

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
