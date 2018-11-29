using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CupApp.Models;
using CupApp.Presentation;
using System.IO;
using WebApplication.Services;

namespace CupApp.Controllers
{
    public class CupsController : Controller
    {
        private readonly CupContext db;
        private readonly IViewRenderService _viewRenderService;

        public CupsController(CupContext context, IViewRenderService viewRenderService)
        {
            db = context;
            _viewRenderService = viewRenderService;
        }

        public async Task<IActionResult> Index()
        {
            var cupContext = db.Cups.Include(c => c.Country);
            ViewData["CountryID"] = new SelectList(db.Countries, "CountryId", "CountryName");
            return View(await cupContext.ToListAsync());
        }
        public ActionResult ViewAll()
        {
            return View(GetAllCupsAsync());
        }

        IEnumerable<Cup> GetAllCupsAsync()
        {
            var cupContext = db.Cups.Include(c => c.Country);
            return cupContext.ToList<Cup>();
        }

        public async Task<IActionResult> AddOrEdit(int? id)
        {
            Cup c = new Cup();
            if (id != null && id != 0)
            {
                c = await db.Cups.Include(cu => cu.Country).SingleOrDefaultAsync(m => m.CupId == id);

                ViewBag.CountryID = new SelectList(db.Countries, "CountryId", "CountryName", c.CountryID);
            }

            else ViewBag.CountryID = new SelectList(db.Countries, "CountryId", "CountryName");
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditCup(Cup cup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (cup.CupId == 0)
                    {
                        db.Cups.Add(cup);
                        db.SaveChanges();
                        if (cup.ImageUpload != null)
                        {
                            var cupImg = new CupImage();
                            cupImg.CupImageID = cup.CupId;
                            cupImg.Image = new byte[cup.ImageUpload.Length];
                            using (var ms = new MemoryStream())
                            {
                                cup.ImageUpload.CopyTo(ms);

                                cupImg.Image = ms.ToArray();
                            }

                            cupImg.Cup = cup;
                            db.CupImages.Add(cupImg);
                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        if (cup.ImageUpload != null)
                        {
                            var img = new byte[cup.ImageUpload.Length];
                            using (var ms = new MemoryStream())
                            {
                                cup.ImageUpload.CopyTo(ms);
                                img = ms.ToArray();
                            }

                            var cupImg = await db.CupImages.SingleOrDefaultAsync(m => m.CupImageID == cup.CupId);
                            if (cupImg == null)
                            {
                                cupImg = new CupImage {CupImageID = cup.CupId, Image = img};
                                db.CupImages.Add(cupImg);
                                await db.SaveChangesAsync();
                            }
                            else
                            {
                                cupImg.Image = img;
                                db.Update(cupImg);
                                await db.SaveChangesAsync();
                            }
                        }

                        db.Update(cup);
                        await db.SaveChangesAsync();
                    }
                    ViewBag.CountryID = new SelectList(db.Countries, "CountryId", "CountryName", cup.CountryID);
                    var result = await _viewRenderService.RenderToStringAsync("Cups/ViewAll", GetAllCupsAsync());
                    return Json(new {success = true, result, message = "Operation was Successfully"});
                }
                else
                {
                    var result = await _viewRenderService.RenderToStringAsync("Cups/ViewAll", GetAllCupsAsync());
                    return Json(new { success = false, result, message = "Can't add, because input values are invalid" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var cupImg = await db.CupImages.SingleOrDefaultAsync(m => m.CupImageID == id);
                if (cupImg != null) db.CupImages.Remove(cupImg);
                var cup = await db.Cups.SingleOrDefaultAsync(m => m.CupId == id);
                db.Cups.Remove(cup);
                await db.SaveChangesAsync();
                ViewBag.CountryID = new SelectList(db.Countries, "CountryId", "CountryName", cup.CountryID);
                var result = await _viewRenderService.RenderToStringAsync("Cups/ViewAll", GetAllCupsAsync());
                return Json(new { success = true, result, message = "Deleted Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

       // [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GetImage(int id)
        {
            Response.Headers.Add("Cache-Control", "no-store");
            CupImage c = await db.CupImages.SingleOrDefaultAsync(m => m.CupImageID == id);
            if (c != null && c.Image != null) return File(c.Image, "image/png");
            return GetDefaultImage();
        }

        private IActionResult GetDefaultImage()
        {
            string path = Directory.GetCurrentDirectory();
            string fileName = path + "/AppFiles/Images/default.png";
            byte[] array = System.IO.File.ReadAllBytes(fileName);
            return File(array, "image/png");
        }
    }
}