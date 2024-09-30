using Microsoft.AspNetCore.Mvc;
using SGMED_UNAPEC.Models;
using SGMED_UNAPEC.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;



public class AccesoController : Controller
{
    private readonly DA_Usuario _da_usuario;



    public AccesoController(DA_Usuario da_usuario)
    {
        _da_usuario = da_usuario;
    }



    public IActionResult Index()
    {
        return View();
    }



    [HttpPost]
    public async Task<IActionResult> Index(Usuario _usuario)
    {
        var usuario = _da_usuario.ValidarUsuario(_usuario.Username, _usuario.Password);



        if (usuario != null)     
        {
           
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.RolId.ToString()),
            };



            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);



            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            ViewBag.Username = usuario.Username;

            

            return RedirectToAction("Index", "Home");
        }
        else
        {
            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }
    }



    public async Task<IActionResult> Salir()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Response.Headers["Cache-Control"] = "no-store";
        return RedirectToAction("Index");
    }
}

