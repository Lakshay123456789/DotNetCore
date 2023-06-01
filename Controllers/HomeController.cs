using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using TestCore.Models;

namespace TestCore.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Movie> movies = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7130/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var responseTask = client.GetAsync("api/CRUD/all");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask =  result.Content.ReadAsStringAsync();
                    var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Movie>>(readTask.Result);
                    readTask.Wait();
                    movies = deserialized;
                }
                else
                {
                    movies = Enumerable.Empty<Movie>();
                    ModelState.AddModelError(string.Empty, "Employees not found.");
                }
            }
            return View(movies);
        }
        [HttpGet]
        public IActionResult AddMovie()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddMovie(Movie movies)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7130/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var postTask = client.PostAsJsonAsync<Movie>("api/CRUD/add", movies);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Failed Try again.");
            return View(movies);
        }
    }
}