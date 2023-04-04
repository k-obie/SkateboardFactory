using Microsoft.AspNetCore.Mvc;
//using SkateFactory4.Data;
//using SkateFactory4.Models;
using SkateFactory4.WebUI.WebApiClients;
using System.Net.Http.Headers;
using SkateFactory4.WebUI.Models;
using Microsoft.AspNetCore.Authorization;

namespace SkateFactory4.WebUI.Controllers;

[Authorize]
public class ManageSkateboardsController : Controller
{
    //private readonly SkateFactoryContext _context;
    private readonly IConfiguration _configuration;
    private SkateFactory4WebAPIClient _webApiClient;

    //public ManageSkateboardsController(SkateFactoryContext context)
    //{
    //    _context = context;
    //}
    public ManageSkateboardsController(IConfiguration configuration)
    {
        _configuration = configuration;

        string serverAddress = configuration["SkateFactoryWebApiServer"];
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(serverAddress);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _webApiClient = new SkateFactory4WebAPIClient(httpClient);
    }


    //[HttpGet] //Optional
    //public IActionResult Index()
    //{
    //    var listOfSkateboards = new List<Skateboard>();
    //    try
    //    {
    //        listOfSkateboards = _context.Skateboards.ToList();
    //    }
    //    catch (Exception ex)
    //    {
    //        ViewBag.Message = new MessageViewModel(EMessageType.Danger, ex.Message);
    //    }
    //    return View(listOfSkateboards);
    //}

    public async Task<IActionResult> Index()
    {
        var listOfSkateboards = new List<Skateboard>();
        try
        {
            listOfSkateboards = (List<Skateboard>)await _webApiClient.SkateboardAllAsync(ESkateboardCriteria.All);
        }
        catch (Exception ex)
        {
            ViewBag.Message = new MessageViewModel(EMessageType.Danger, ex.Message);
        }
        //var result = listOfSkateboards.Select(s => s.ToSkateboardVM()).ToList();
        //return View(result);
        return View(listOfSkateboards);
    }

    //[HttpGet] //Optional
    //public IActionResult AddOrUpdate(int? id)
    //{
    //    if (id == null)
    //        return View();

    //    Skateboard? skateboard = null;

    //    try
    //    {
    //        skateboard = _context.Skateboards.Find(id);
    //        if (skateboard == null)
    //        {
    //            return View("NotFound");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ViewBag.Message = new MessageViewModel(EMessageType.Danger, ex.Message);
    //    }

    //    return View(skateboard);
    //}

    public async Task<IActionResult> AddOrUpdate(int? id)
    {
        if (id == null)
            return View();

        Skateboard? skateboard = null;
        try
        {
            skateboard = await _webApiClient.SkateboardGETAsync(id.Value);
            if (skateboard == null)
                return View("NotFound");
        }
        catch (Exception ex)
        {
            ViewBag.Message = new MessageViewModel(EMessageType.Danger, ex.Message);
        }
        return View(skateboard);
    }

    //[HttpPost]
    //public IActionResult AddOrUpdate(Skateboard skateboard)
    //{
    //    if (!ModelState.IsValid)
    //        return (skateboard.Id == 0) ? View() : View(skateboard);

    //    try
    //    {
    //        if (skateboard.Id == 0)
    //            _context.Add(skateboard);
    //        else
    //            _context.Update(skateboard);
    //        _context.SaveChanges();
    //    }
    //    catch (Exception ex)
    //    {
    //        ViewBag.Message = new MessageViewModel(EMessageType.Danger, ex.Message);
    //        return (skateboard.Id == 0) ? View() : View(skateboard);
    //    }
    //    return RedirectToAction("Index");
    //}

    [HttpPost]
    public async Task<IActionResult> AddOrUpdate(Skateboard skateboard)
    {
        if (!ModelState.IsValid)
            return (skateboard.Id == 0) ? View() : View(skateboard);

        try
        {
            if (skateboard.Id == 0)
                await _webApiClient.SkateboardPOSTAsync(skateboard);
            else
                await _webApiClient.SkateboardPUTAsync(skateboard);
        }

        catch (Exception ex)
        {
            ViewBag.Message = new MessageViewModel(EMessageType.Danger, ex.Message);
            return (skateboard.Id == 0) ? View() : View(skateboard);
        }
        return RedirectToAction(nameof(Index));
    }

    //[HttpPost]
    //public IActionResult Remove(int id)
    //{
    //    try
    //    {
    //        var skateboard = _context.Skateboards.Find(id);
    //        if (skateboard != null)
    //        {
    //            _context.Skateboards.Remove(skateboard);
    //            _context.SaveChanges();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ViewBag.Message = new MessageViewModel(EMessageType.Danger, ex.Message);
    //    }
    //    return RedirectToAction("Index");
    //}

    [HttpPost]
    public async Task<IActionResult> Remove(int id)
    {
        try
        {
            var skateboard = await _webApiClient.SkateboardGETAsync(id);
            if (skateboard != null)
                await _webApiClient.SkateboardDELETEAsync(id);
        }
        catch (Exception ex)
        {
            ViewBag.Message = new MessageViewModel(EMessageType.Danger, ex.Message);
        }

        return RedirectToAction(nameof(Index));
    }
}
