using EssayCheek.Api.Model.Foundation.EssayResult;
using EssayCheek.Api.Services.EssayResults;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace EssayCheek.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class EssayResultController : RESTFulController
{
    private readonly IEssayResultService _sayResultService;

    public EssayResultController(IEssayResultService sayResultService) =>
        _sayResultService = sayResultService;

    [HttpPost]
    public async ValueTask<ActionResult<EssayResult>> PostEssayResult(EssayResult essayResult)
    {
        EssayResult result = await _sayResultService.AddEssayResultsAsync(essayResult);
        return Ok(result);
    }

    [HttpGet]
    public ActionResult<IQueryable<EssayResult>> GetAllEssayResults()
    {
        IQueryable<EssayResult> retrievedResults = _sayResultService.RetrieveAllEssayResults();
        return Ok(retrievedResults);
    }

    [HttpDelete]
    public async ValueTask<ActionResult<EssayResult>> DeleteEssayResultsByIdAsync(EssayResult essayResult)
    {
        EssayResult result = await _sayResultService.RemoveEssayResultsAsync(essayResult);
        return Ok(result);
    }
}