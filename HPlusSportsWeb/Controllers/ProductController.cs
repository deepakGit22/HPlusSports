using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HPlusSportsWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("localHost");
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            //var request = new HttpRequestMessage(HttpMethod.Get,
            //"product");
            //var client = _httpClientFactory.CreateClient("localHost");
            string baseUri = _httpClient.BaseAddress.AbsoluteUri;

            var response = await _httpClient.GetStringAsync($"{baseUri}product");
            var products = JArray.Parse(response);

            return View(products);
        }

        public async Task<IActionResult> Detail(int id)
        {
            //var request = new HttpRequestMessage(HttpMethod.Get,
            //$"product/{id}");

            string baseUri = _httpClient.BaseAddress.AbsoluteUri;
            var response = await _httpClient.GetStringAsync($"{baseUri}product/{id}");

            //var client = _httpClientFactory.CreateClient();
            //var response = await client.GetStringAsync($"https://localhost:44350/api/product/{id}");

            var product = JsonConvert.DeserializeObject<Models.Product>(response);
            return View(product);
        }
    }
}
