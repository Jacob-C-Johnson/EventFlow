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

    [HttpPost]
    [Route("/GetReservation")]
    public async Task<IActionResult> GetReservation(int reservationid)
    {
        var reservation = await _eventFlowRepository.GetReservation(reservationid);   
        return Json(new { reservation });
    }

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
