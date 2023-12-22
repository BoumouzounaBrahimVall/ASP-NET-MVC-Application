using AmazonCloneMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AmazonCloneMVC.Controllers
{
    [Authorize]
    public class ProduitsController : Controller
    {
        private readonly MyDbContext _context;

        public ProduitsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Produits
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.Produits.Include(p => p.Categorie);
            return View(await myDbContext.ToListAsync());
        }

        // GET: Produits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Produits == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits
                .Include(p => p.Categorie)
                .FirstOrDefaultAsync(m => m.ProduitID == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // GET: Produits/Create
        public IActionResult Create()
        {
            ViewData["CategorieID"] = new SelectList(_context.Categories, "CategorieID", "NomCategorie");


            return View();
        }

        // POST: Produits/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProduitID,ProduitName,Description,Prix,Quantite,ImagePath,CategorieID")] Produit produit, IFormFile file)
        {
          //  if (ModelState.IsValid)
         //   {
                if (file != null)
                {
                   
                    // Save the image to a directory on the server
                    var fileName = Path.GetFileName(file.FileName);
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);


                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    // Update the product's image path in the database
                    produit.ImagePath = "images/" + fileName;
                }
                _context.Add(produit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
          
        
        }

        // GET: Produits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Produits == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }
            ViewData["CategorieID"] = new SelectList(_context.Categories, "CategorieID", "NomCategorie", produit.CategorieID);
            return View(produit);
        }

        // POST: Produits/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProduitID,ProduitName,Description,Prix,Quantite,ImagePath,CategorieID")] Produit produit)
        {
            if (id != produit.ProduitID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduitExists(produit.ProduitID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategorieID"] = new SelectList(_context.Categories, "CategorieID", "NomCategorie", produit.CategorieID);
            return View(produit);
        }

        // GET: Produits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Produits == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits
                .Include(p => p.Categorie)
                .FirstOrDefaultAsync(m => m.ProduitID == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // POST: Produits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Produits == null)
            {
                return Problem("Entity set 'MyDbContext.Produits'  is null.");
            }
            var produit = await _context.Produits.FindAsync(id);
            if (produit != null)
            {
                _context.Produits.Remove(produit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduitExists(int id)
        {
            return (_context.Produits?.Any(e => e.ProduitID == id)).GetValueOrDefault();
        }
    }
}
