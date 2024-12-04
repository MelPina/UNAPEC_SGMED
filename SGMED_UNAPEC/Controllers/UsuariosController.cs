using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGMED_UNAPEC.Models;

namespace SGMED_UNAPEC.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly sgmed_unapecContext _context;

        public UsuariosController(sgmed_unapecContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Index()
        {
            //var sgmed_unapecContext = _context.Usuarios.Include(u => u.Estado).Include(u => u.Rol);
            //return View(await sgmed_unapecContext.ToListAsync());

            return _context.Usuarios != null ?
                       View(await _context.Usuarios.ToListAsync()) :
                       Problem("Entity set 'sgmed_unapecContext.Usuarios'  is null.");
        }
        [HttpPost]
        public JsonResult ObtenerDatosUsuarios()
        {
            try
            {
                // Representa el número de veces que se ha realizado una petición
                int NroPeticion = Convert.ToInt32(Request.Form["draw"].FirstOrDefault() ?? "0");

                // Cuantos registros va a devolver
                int CantidadRegistros = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");

                // Cuantos registros va a omitir
                int OmitirRegistros = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

                // El texto de búsqueda
                string ValorBuscado = Request.Form["search[value]"].FirstOrDefault() ?? "";

              
                IQueryable<Usuario> queryTbLogin = _context.Usuarios;

            
                queryTbLogin = queryTbLogin.Include(u => u.Estado).Include(u => u.Rol);

                // Filtro de búsqueda
                if (!string.IsNullOrEmpty(ValorBuscado))
                {
                    queryTbLogin = queryTbLogin.Where(e =>
                        e.UsuarioId.ToString().Contains(ValorBuscado) ||
                        e.Nombre.Contains(ValorBuscado) ||
                        e.Username.Contains(ValorBuscado) ||
                        e.Estado.Descripcion.Contains(ValorBuscado) ||  
                        e.Rol.Descripcion.Contains(ValorBuscado));      
                }

                // Total de registros antes de filtrar.
                int TotalRegistros = _context.Usuarios.Count();

                // Total de registros filtrados.
                int TotalRegistrosFiltrados = queryTbLogin.Count();

                // Obtener registros paginados
                var listaUsuarios = queryTbLogin
                    .Skip(OmitirRegistros)
                    .Take(CantidadRegistros)
                    .Select(u => new
                    {
                        u.UsuarioId,
                        u.Nombre,
                        u.Username,
                        Estado = u.Estado.Descripcion, 
                        Rol = u.Rol.Descripcion        
                    })
                    .ToList();

                // Devolver respuesta JSON esperada por DataTables
                return Json(new
                {
                    draw = NroPeticion,
                    recordsTotal = TotalRegistros,
                    recordsFiltered = TotalRegistrosFiltrados,
                    data = listaUsuarios // Esta propiedad debe llamarse "data"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = "Ocurrió un error al obtener los datos: " + ex.Message
                });
            }
        }

     

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Estado)
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }


        // GET: Usuarios/Create
        public IActionResult Create()
        {
            //// Obtener los estados y roles desde la base de datos
            //var estados = _context.Estados.Select(e => new { e.EstadoId, e.Descripcion }).ToList();
            //var roles = _context.Rols.Select(r => new { r.RolId, r.Descripcion }).ToList();

            //// Poblar los ViewBag con las listas de estados y roles
            //ViewBag.EstadoId = new SelectList(estados, "EstadoId", "Descripcion");
            //ViewBag.RolId = new SelectList(roles, "RolId", "Descripcion");

            var rol = _context.Rols.ToList();
            ViewBag.Rol = rol;

            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;

            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,Nombre,Username,Password,RolId,EstadoId")] Usuario usuario)
        {
            
            {
                try
                {
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Captura el error para propósitos de depuración
                    ModelState.AddModelError("", "Ocurrió un error al crear el usuario: " + ex.Message);
                }
            }

            //// Si falla la validación, recargamos los ViewBag para mostrarlos de nuevo en la vista
            //ViewBag.EstadoId = new SelectList(_context.Estados, "EstadoId", "Descripcion", usuario.EstadoId);
            //ViewBag.RolId = new SelectList(_context.Rols, "RolId", "Descripcion", usuario.RolId);

            var rol = _context.Rols.ToList();
            ViewBag.Rol = rol;

            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;

            return View(usuario);
        }


        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", usuario.EstadoId);
            ViewData["RolId"] = new SelectList(_context.Rols, "RolId", "RolId", usuario.RolId);

            var rol = _context.Rols.ToList();
            ViewBag.Rol = rol;

            var estado = _context.Estados.ToList();
            ViewBag.Estado = estado;
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,Nombre,Username,Password,RolId,EstadoId")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
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
            //var rol = _context.Rols.ToList();
            //ViewBag.Rol = rol;

            //var estado = _context.Estados.ToList();
            //ViewBag.Estado = estado;

            //// Loguear los errores del ModelState si no es válido
            //foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            //{
            //    Console.WriteLine(error.ErrorMessage);
            //}

            //ViewData["EstadoId"] = new SelectList(_context.Estados, "EstadoId", "EstadoId", usuario.EstadoId);
            //ViewData["RolId"] = new SelectList(_context.Rols, "RolId", "RolId", usuario.RolId);
            //return View(usuario);
        }


        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Estado)
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'sgmed_unapecContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.UsuarioId == id)).GetValueOrDefault();
        }
    }
}
