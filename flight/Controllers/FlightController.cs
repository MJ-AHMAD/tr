using flight.Models;
using flight.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace flight.Controllers
{
    public class FlightController : Controller
    {
        private readonly FlightService _flightService;

        public FlightController(FlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(FlightSearchModel model)
        {
            var flights = await _flightService.SearchFlightsAsync(model.Origin, model.Destination, model.DepartureDate.ToString("yyyy-MM-dd"));
            ViewBag.Flights = flights;
            return View();
        }
    }
}
