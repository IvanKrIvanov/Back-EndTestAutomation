using Microsoft.AspNetCore.Mvc;
using MVCIntroDemo.Models.Product;
using System.Text;
using System.Text.Json;

namespace MVCIntroDemo.Controllers
{
    public class ProductController : Controller
    {
        private List<ProductViewModels> _products = new List<ProductViewModels>()
        {
            new ProductViewModels()
            {
                Id = 1,
                Name = "Cheese",
                Price = 7.0m
            },
            new ProductViewModels()
            {
                Id = 2,
                Name = "Ham",
                Price = 5.5m
            },
            new ProductViewModels()
            {
                Id = 3,
                Name = "Bread",
                Price = 1.5m
            }
        };
        public IActionResult All()
        {
            return View(_products);
        }
        public IActionResult ById( int Id)
        {
            var products = _products.FirstOrDefault(x => x.Id == Id);
            if (products ==null)
            {
                return NotFound();
            }
            return View();
        }
		public IActionResult AllAsJosn()
		{
            return Json(_products, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
			
		}

        public IActionResult AllAsText()
        {
            var text = string.Empty;
            foreach (var item in _products)
            {
                text += $"Product {item.Id}: {item.Name} - {item.Price} lv.{Environment.NewLine}";
               
            }
			return Content(text);
		}

	}
}
