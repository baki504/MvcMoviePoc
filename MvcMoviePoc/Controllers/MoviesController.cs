using Microsoft.AspNetCore.Mvc;
using MvcMoviePoc.Services;
using MvcMoviePoc.ViewModels;

namespace MvcMoviePoc.Controllers
{
    public class MoviesController(MovieService service) : Controller
    {
        private readonly MovieService _service = service;

        // GET: Movies
        public async Task<IActionResult> Index(string movieGenre, string searchString)
        {
            MovieListViewModel vm = await _service.GetListAsync(searchString, movieGenre, null);
            return View(vm);
        }

        [HttpPost]
        public string Index(string searchString)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == null)
            {
                return NotFound();
            }

            MovieItemViewModel? vm = await _service.GetAsync(id.Value);
            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            MovieItemViewModel vm = new MovieItemViewModel();
            return View(vm);
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieItemViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            int id = await _service.CreateAsync(vm);
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == null)
            {
                return NotFound();
            }

            MovieItemViewModel? vm = await _service.GetAsync(id.Value);
            if (vm == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieItemViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            bool updated = await _service.UpdateAsync(vm);
            if (!updated)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id == null)
            {
                return NotFound();
            }

            MovieItemViewModel? vm = await _service.GetAsync(id.Value);
            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
