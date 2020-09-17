using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskN1.Data;
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
            var person = from m in _context.Person
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                person = person.Where(s => s.Name.Contains(searchString)
                                     | s.Surname.Contains(searchString)
                                     | s.PersonalID.Contains(searchString));
            }
            return View(await person.ToListAsync());
        }

        //// GET: People
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Person.ToListAsync());
        //}

        // GET: People/Details/5
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

        // GET: People/Create
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
                if ((DateTime.Now.Year - person.PersonBirthDate.Year) > 18)
                {
                    //save image to wwwRoot
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(person.ImageFile.FileName);
                    string extension = Path.GetExtension(person.ImageFile.FileName);
                    person.Picture=fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                    using (var filestream = new FileStream(path,FileMode.Create))
                    {
                        await person.ImageFile.CopyToAsync(filestream);
                    }
                        _context.Add(person);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else {ViewData["Message"] = "გთხოვთ მიუთითოთ სწორი დაბადების წელი, მინიმალური ასაკი=18";}
            }
            else{ }
            return View(person);
        }

       
        // GET: People/Edit/5
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

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Surname,Sex,PersonalID,BirthDate,PersonBirthDate,City,Mobile,ImageFile,ConnectedPeople")] Person person)
        {
            if (id != person.ID)
            {
                return NotFound();
            }

                if (ModelState.IsValid && person.ImageFile!=null)
                {

                //save image to wwwRoot
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(person.ImageFile.FileName);
                string extension = Path.GetExtension(person.ImageFile.FileName);
                person.Picture = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
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
            return View(person);
        }

        // GET: People/Delete/5
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

        // POST: People/Delete/5
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
