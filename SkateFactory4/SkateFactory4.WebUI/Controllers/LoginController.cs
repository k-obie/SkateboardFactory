using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SkateFactory4.WebUI.Models;
using SkateFactory4.WebUI.WebApiClients;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace SkateFactory4.WebUI.Controllers;

public class LoginController : Controller
{
	private readonly IConfiguration _configuration;
	private SkateFactory4WebAPIClient _webApiClient;

	public LoginController(IConfiguration configuration)
	{
		_configuration = configuration;

		var httpClient = new HttpClient();
		string serverAddress = configuration["SkateFactoryWebApiServer"];
		httpClient.BaseAddress = new Uri(serverAddress);
		httpClient.DefaultRequestHeaders.Accept.Clear();
		httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		_webApiClient = new SkateFactory4WebAPIClient(httpClient);
	}

	public IActionResult Index()
	{
		ViewBag.User = new UserViewModel();
		ViewBag.RegisterUser = new RegisterUserViewModel();
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Register(RegisterUserViewModel registerUserVm)
	{
		if (!ModelState.IsValid)
		{
			ViewBag.User = new UserViewModel();
			ViewBag.RegisterUser = registerUserVm;
			return View();
		}

		try
		{
			var user = new User()
			{
				Email = registerUserVm.NewEmail,
				Password = registerUserVm.Password
			};

			var listOfErrors = await _webApiClient.RegisterAsync(user);

			if (listOfErrors.Count == 0)
				ViewBag.Message = new MessageViewModel(EMessageType.Success, $"{user.Email} has been created as a new user and is ready to sign in");
			else
			{
				string errorMessage = string.Join("<br>", listOfErrors);
				ViewBag.Message = new MessageViewModel(EMessageType.Danger, errorMessage);
			}
		}
		catch (Exception ex)
		{
			ViewBag.Message = new MessageViewModel(EMessageType.Danger, ex.Message);
		}

		return View("Index");
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> SignIn(UserViewModel userVm)
	{
		if (!ModelState.IsValid)
		{
			ViewBag.User = userVm;
			ViewBag.RegisterUser = new RegisterUserViewModel();
			return View();
		}

		try
		{
			var user = new User()
			{
				Email = userVm.ExistingEmail,
				Password = userVm.Password
			};
			bool success = await _webApiClient.UserandpasswordarevalidAsync(user);

			if (success)
			{
				var claims = new[] { new Claim(ClaimTypes.Name, user.Email) };
				var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
				return RedirectToAction("Index", "ManageSkateboards");
			}
			else
				ViewBag.Message = new MessageViewModel(EMessageType.Danger, "Invalid e-mail or password");
		}
		catch (Exception ex)
		{
			ViewBag.Message = new MessageViewModel(EMessageType.Danger, ex.Message);
		}

		return View("Index");
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> SignOutAndGoToHome()
	{
		await HttpContext.SignOutAsync();
		return RedirectToAction("Index", "Home");
	}
}
