using DAL.Entities;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : Controller
    {


        AppDbContext _db;
        public CategoryController(AppDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        public IEnumerable<Category> GetAll()
        {
            return _db.Categories.ToList(); //default status code : 200 + data +datatype
        }
    }    
}   
