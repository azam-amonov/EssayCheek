using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Services.Essays;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace EssayCheek.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class EssaysController : RESTFulController
{
    private readonly IEssayService _essayService;

    public EssaysController(IEssayService essayService) =>
        _essayService = essayService;

    [HttpPost]
    public async ValueTask<ActionResult<Essay>> PostEssayAsync ([FromBody] Essay essay)
    {
        Essay addedEssay = await _essayService.AddEssayAsync(essay);
        return Created(addedEssay);
    }

    [HttpGet]
    public ActionResult<IQueryable<Essay>> GetAllEssays()
    {
        IQueryable<Essay> essays = _essayService.RetrieveAllEssays();
        return Ok(essays);
    }

    [HttpGet("{guid}")]
    public async ValueTask<ActionResult<Essay>> GetEssayById([FromForm] Guid essayId)
    {
        Essay? retrievedEssay = await _essayService.RetrieveEssayByIdAsync(essayId);
        return Ok(retrievedEssay);
    }

    [HttpDelete("{guid}")]
    public async ValueTask<ActionResult<Essay>> DeleteEssayById(Guid essayId)
    {
        Essay retrievedEssay = await _essayService.RemoveEssayByIdAsync(essayId);
        return Ok(retrievedEssay);
    }
}