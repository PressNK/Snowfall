using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Snowfall.Web.Mvc.Controllers;

public class LocaleController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    /// <summary>
    /// Permets de changer la locale de l'utilisateur et de configurer le cookie de préférence
    /// </summary>
    /// <param name="culture"></param>
    /// <param name="urlRetour"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task<IActionResult> Update(string culture, string urlRetour)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        return LocalRedirect(urlRetour);
    }
}