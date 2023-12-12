using EssayCheek.Api.Brokers.OpenAis;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace EssayCheek.Api.Controllers;

[ApiController]
[Route("api[controller]")]
public class HomeController : RESTFulController
{
	private readonly IOpenAiBroker _openAiBroker;

	public HomeController(IOpenAiBroker openAiBroker)
	{
		_openAiBroker = openAiBroker;
	}

	[HttpGet]
	public ActionResult<string> GetHomeMessage() => Ok("Tarteeb is running");

	[HttpPost]
	public async ValueTask<ActionResult<ChatCompletion>> Post(ChatCompletion chatCompletion)
	{
		return await _openAiBroker.AnalyzeEssayAsync(chatCompletion);
	}
}