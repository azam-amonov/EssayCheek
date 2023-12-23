using EssayCheek.Api.Services.TelegramBots;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace EssayCheek.Api.Controllers;

public class BotController : RESTFulController
{
    private readonly ITelegramBotService telegramBotService;

    public BotController(ITelegramBotService telegramBotService)
    {
        this.telegramBotService = telegramBotService;
    }
    [HttpGet("/start-bot")]
    public async Task<IActionResult> StartBot()
    {
        await this.telegramBotService.BotStartAsync();
        return Ok("Bot started!");
    }
}