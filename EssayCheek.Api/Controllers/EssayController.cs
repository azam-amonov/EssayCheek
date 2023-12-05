using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Services.Essays;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace EssayCheek.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class EssayController : RESTFulController
{
    private readonly IEssayService _essayService;

    public EssayController(IEssayService essayService) =>
        _essayService = essayService;

    [HttpPost]
    public async ValueTask<ActionResult<Essay>> PostEssayAsync(Essay essay)
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

    [HttpGet("get/{guid}")]
    public async ValueTask<ActionResult<Essay>> GetEssayById(Guid essayId)
    {
        Essay? retrievedEssay = await _essayService.RetrieveGetByIdAsync(essayId);
        return Ok(retrievedEssay);
    }

    [HttpDelete("delete/{guid}")]
    public async ValueTask<ActionResult<Essay>> DeleteEssayById(Guid essayId)
    {
        Essay retrievedEssay = await _essayService.RemoveEssayAsync(essayId);
        
        return Ok(retrievedEssay);
    }
    
}