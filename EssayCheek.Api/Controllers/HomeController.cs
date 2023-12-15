using EssayCheek.Api.Services.EssayAnalysis;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace EssayCheek.Api.Controllers;

[ApiController]
[Route("api[controller]")]
public class HomeController : RESTFulController
{
	private readonly IEssayAnalysisService _essayAnalysisService;

	public HomeController(IEssayAnalysisService essayAnalysisService)
	{
		_essayAnalysisService = essayAnalysisService;
	}

	[HttpGet]
	public ActionResult<string> GetHomeMessage() => Ok("Tarteeb is running");

	[HttpPost]
	public async ValueTask<ActionResult<string>> Post([FromBody] string essay)
	{
		var result = await _essayAnalysisService.EssayAnalysisAsync(essay);
		return Ok(result);
	}
}