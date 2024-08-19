using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {

        HttpClient _client;
        IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_configuration["ApiAddress"]);
        }


        //public IActionResult Index()
        //{
        //    IEnumerable<ProductViewModel> products = null;
        //    var response = _client.GetAsync(_client.BaseAddress + "/Product/GetAll").Result;

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string data = response.Content.ReadAsStringAsync().Result;
        //        products = JsonSerializer.Deserialize<IEnumerable<ProductViewModel>>(data);


        //    }
        //    return View(products);
        //}


        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductViewModel> products = null;
            var response = await _client.GetAsync(_client.BaseAddress + "/Product/GetAll");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                products = JsonSerializer.Deserialize<IEnumerable<ProductViewModel>>(data);


            }
            return View(products);
        }


        private IEnumerable<CategoryViewModel> GetCategories()
        {
            IEnumerable<CategoryViewModel> Categories = null;
            var response = _client.GetAsync(_client.BaseAddress + "/Category/GetAll").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Categories = JsonSerializer.Deserialize<IEnumerable<CategoryViewModel>>(data);

            }
            return Categories;
        }



        public IActionResult Create()
        {
            ViewBag.Categories = GetCategories();
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel product)
        {
            string jsonData = JsonSerializer.Serialize(product);
            var content = new StringContent(jsonData,Encoding.UTF8,"application/json");

            var response = _client.PostAsync(_client.BaseAddress + "/Product/Add", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(product);

        }


        public  IActionResult Edit(int id)
        {
            ProductViewModel product = null;
            var response = _client.GetAsync(_client.BaseAddress + "/Product/Get/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                product = JsonSerializer.Deserialize<ProductViewModel>(data);


            }
            ViewBag.Categories = GetCategories();
            return View(product);

        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel product)
        {
            string jsonData = JsonSerializer.Serialize(product);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = _client.PutAsync(_client.BaseAddress + "/Product/update/"+product.ProductId, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(product);

        }

        public IActionResult Delete(int id)
        {

            var response = _client.DeleteAsync(_client.BaseAddress + "/Product/Delete/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

    }
}
