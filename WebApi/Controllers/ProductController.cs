using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        AppDbContext _db;
        public ProductController(AppDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        public IEnumerable<Product> GetAll()
        {
            return _db.Products.ToList(); //default status code : 200 + data +datatype
        }


        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [HttpGet]
        public IActionResult GetProducts()
        {
            var data = _db.Products.ToList();
            return StatusCode(StatusCodes.Status200OK,data); // custom status code: 200 + data
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProductsList()
        {
            var data = _db.Products.ToList();
            return StatusCode(StatusCodes.Status200OK, data); // custom status code: 200 + data + datatype
        }

        [HttpGet("{id}")]
        public Product Get(int id) 
        {
            return _db.Products.Find(id); //default status code : 200 + data
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public IActionResult Add(Product product)
        {
            try
            {
                _db.Products.Add(product);
                _db.SaveChanges();
                return StatusCode(StatusCodes.Status201Created); //Custom Status code :201+data
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public IActionResult Update(int id, Product model)
        {
            
            try
            {
                if (id != model.ProductId)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "ID is not valid");
                }
                _db.Update(model);
                _db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, model);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Product),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string),StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var product = _db.Products.Find(id);
                if (product != null)
                {
                    _db.Products.Remove(product);
                    _db.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK, product);
                    

                }
                return StatusCode(StatusCodes.Status404NotFound, "Product not found");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        

    
    
    
    
    
    }
}
