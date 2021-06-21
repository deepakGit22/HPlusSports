using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HPlusSportsWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HPlusSportsWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("localHost");
        }

        //Add Product
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddNutritional(NutritionalProduct newProduct)
        {
            //Call API to add new product
            var response =
                await _httpClient.PostAsJsonAsync<NutritionalProduct>("product/Nutritional", newProduct);

            //check response for errors
            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<NutritionalProduct>();
                ViewData["NutritionalModel"] = model;
                ViewData["NewProductId"] = model.Id;
                ViewData["ProgressMessage"] = "Product Created";
                return View("Index");
            }
            else
            {
                throw new ApplicationException(response.ReasonPhrase);
            }

        }

        public async Task<IActionResult> AddClothing(ClothingProduct newProduct)
        {
            //Call API to add new product
            var response =
                await _httpClient.PostAsJsonAsync<ClothingProduct>("product/Clothing", newProduct);

            //check response for errors
            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<ClothingProduct>();
                ViewData["ClothingModel"] = model;
                ViewData["NewProductId"] = model.Id;
                ViewData["ProgressMessage"] = "Product Created";
                return View("Index");
            }
            else
            {
                throw new ApplicationException(response.ReasonPhrase);
            }
        }

        public async Task<IActionResult> NewImage(IFormFile imageFile)
        {
            //product ID from query string
            string id = Request.Form["imageProductId"];
            if (imageFile.Length > 0)
            {
                //create form payload to pass to web API
                var imageContent = new StreamContent(imageFile.OpenReadStream());
                imageContent.Headers.ContentDisposition =
                    new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        Name = "imageFile",
                        FileName = imageFile.FileName
                    };

                var postContent = new MultipartFormDataContent();
                postContent.Add(imageContent, "imageFile");

                //call web api passing multipart form data
                var result = await _httpClient.PostAsync($"product/image/{id}", postContent);

                if (result.IsSuccessStatusCode)
                {
                    ViewData["ProgressMessage"] = "Image Added";
                    return View("Index");
                }
                else
                {
                    throw new ApplicationException(result.ReasonPhrase);
                }
            }
            else
            {
                return BadRequest();
            }

        }

        /* add operation for adding image to product
         * this will add the image to blob storage and update the data store with the link */

    }
}