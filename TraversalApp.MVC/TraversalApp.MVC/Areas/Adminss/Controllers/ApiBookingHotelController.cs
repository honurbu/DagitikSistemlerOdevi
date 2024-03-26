﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using TraversalApp.Core.DTOs.APIsDTO;

namespace TraversalApp.MVC.Areas.Adminss.Controllers
{
    [Area("Adminss")]
    [AllowAnonymous]
    public class ApiBookingHotelController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com.p.rapidapi.com/v2/hotels/search?locale=en-gb&filter_by_currency=AED&checkin_date=2024-09-14&dest_type=city&dest_id=-553173&adults_number=2&checkout_date=2024-09-15&order_by=popularity&room_number=1&units=metric&children_number=2&children_ages=5%2C0&categories_filter_ids=class%3A%3A2%2Cclass%3A%3A4%2Cfree_cancellation%3A%3A1&include_adjacency=true&page_number=0"),
                Headers =
    {
        { "X-RapidAPI-Key", "9a654ac3e3mshaf45b14e5ac5acdp134eebjsnff8e9750d288" },
        { "X-RapidAPI-Host", "booking-com.p.rapidapi.com" },
    },
           
        };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ApiBookingHotelDto>(body);
                return View(values?.result); 
            }
        }


        [HttpGet]
        public IActionResult GetCityDestinationId()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> GetCityDestinationId(string city)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://booking-com.p.rapidapi.com/v1/hotels/locations?name={city}&locale=tr"),
                Headers =
    {
        { "X-RapidAPI-Key", "9a654ac3e3mshaf45b14e5ac5acdp134eebjsnff8e9750d288" },
        { "X-RapidAPI-Host", "booking-com.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
            }
            return View();
        }

    }
}
