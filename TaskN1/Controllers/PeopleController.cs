using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;
using TaskN1.Data;
using TaskN1.Migrations;
using TaskN1.Models;

namespace TaskN1.Controllers
{
    public class PeopleController : Controller
    {
        private readonly MvcPersonContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        

        public PeopleController(MvcPersonContext context,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var person = from p in _context.Person
                         select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                person = person.Where(s => s.Name.Contains(searchString)
                                     || s.Surname.Contains(searchString)
                                     || s.PersonalID.Contains(searchString));
            }
            return View(await person.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.ID == id);

            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Surname,Sex,PersonalID,BirthDate,PersonBirthDate,City,Mobile,ImageFile,ConnectedPeople")] Person person)
        {           
            if (ModelState.IsValid)
            {
                int ResultMonth = (DateTime.Now.Month - person.PersonBirthDate.Month),
                    ResultDay = (DateTime.Now.Day - person.PersonBirthDate.Day),
                    ResultYear = (DateTime.Now.Year - person.PersonBirthDate.Year);

                if ((ResultYear > 18 & ResultYear <= 100) || (((ResultMonth >= 0) & (ResultDay >= 0)) & (ResultYear == 18)))
                {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        person.Picture = Path.GetFileName(person.ImageFile.FileName);
                        string path = Path.Combine(wwwRootPath + "/Image/", person.Picture);
                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                            await person.ImageFile.CopyToAsync(filestream);
                        }
                        _context.Add(person);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                }
                else { ViewData["Date"] = "გთხოვთ მიუთითოთ სწორი დაბადების წელი, მინიმალური ასაკი=18"; }
            }
            return View(person);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Surname,Sex,PersonalID,BirthDate,PersonBirthDate,City,Mobile,ImageFile,ConnectedPeople")] Person person)
        {
            if (id != person.ID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                int ResultMonth = (DateTime.Now.Month - person.PersonBirthDate.Month);
                int ResultDay = (DateTime.Now.Day - person.PersonBirthDate.Day);
                int ResultYear = (DateTime.Now.Year - person.PersonBirthDate.Year);

                if ((ResultYear > 18 & ResultYear <= 100) || (((ResultMonth >= 0) & (ResultDay >= 0)) & (ResultYear == 18)))
                {
                   
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        person.Picture = Path.GetFileName(person.ImageFile.FileName);
                        string path = Path.Combine(wwwRootPath + "/Image/", person.Picture);
                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                            await person.ImageFile.CopyToAsync(filestream);
                        }
                    
                    try
                    {
                        _context.Update(person);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PersonExists(person.ID))
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
                else{ ViewData["DateCahnge"] = "ასაკი,შეცვლილია არაკორექტულად, გთხოვთ მიუთითოთ სწორი თარიღი"; }
            }
            return View(person);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.ID == id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
             
            var person = await _context.Person.FindAsync(id);
            if (person.Picture != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", person.Picture);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);

            }
            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.ID == id);
        }
    }  
}
