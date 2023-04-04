using Microsoft.AspNetCore.Mvc;
using SkateFactory4.Models;
namespace SkateFactory4.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EnumsSchemaController : ControllerBase
{
	[HttpGet]
	public ActionResult<EnumSchema> GetEnumSchemas()
	{
		return new EnumSchema();
	}
}