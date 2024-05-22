using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using TevaInventorymanagementsystem.Entity;
using TevaInventorymanagementsystem.Models;

namespace TevaInventorymanagementsystem.Controllers
{
    public class ProductsController : Controller
    {
        private readonly InventoryContext _context;

        public ProductsController(InventoryContext context)
        {
            _context = context;
        }
                
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        public IActionResult Login()
        {
            return View();
        }



        public IActionResult Create()
        {
            return View();
        }

        // GET: Products/Upload
        public IActionResult Upload()
        {
            return View();
        }
        public ActionResult AddNew()
        {
            return PartialView("_CreateProduct");
        }
               

        // POST: Products/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(UploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                var products = new List<Product>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var stream = new MemoryStream())
                {
                    await model.ExcelFile.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.First();
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++) // the first row is the header
                        {
                            var product = new Product
                            {
                                Name = worksheet.Cells[row, 1].Value?.ToString(),
                                Description = worksheet.Cells[row, 2].Value?.ToString(),
                                Quantity = int.Parse(worksheet.Cells[row, 3].Value?.ToString() ?? "0"),
                                CreateDate = DateTime.Now,
                                UpdateDate = DateTime.Now
                            };
                            products.Add(product);
                        }
                    }
                }

                _context.Products.AddRange(products);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Quantity,CreateDate,UpdateDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreateDate = DateTime.Now;
                product.UpdateDate = DateTime.Now;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }



        // GET: Products/Edit/5
        public async Task<IActionResult> EditPartial(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }          
            return PartialView("_EditPartial", product); // Return a partial view
        }
        [HttpPost]


        

        // POST: Products/Edit/5
        [HttpPost]
      
        public JsonResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var productInDb = _context.Products.SingleOrDefault(p => p.Id == product.Id);
                if (productInDb == null)
                {
                    return Json(new { success = false, message = "Product not found." });
                }

                productInDb.Name = product.Name;
                productInDb.Description = product.Description;
                productInDb.Quantity = product.Quantity;
                productInDb.UpdateDate = DateTime.Now;

                _context.SaveChanges();

                return Json(new { success = true, message = "Product updated successfully." });
            }
            return Json(new { success = false, message = "Model state is invalid." });
        }

        // POST: Products/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }
            _context.Products.Remove(product);
            _context.SaveChanges();

            return Json(new { success = true, message = "Product deleted successfully." });
        }

    }
}