using Microsoft.AspNetCore.Mvc;
using ServiceB.Services;

namespace ServiceB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LatestResultController : ControllerBase
    {
        private readonly ITextFileService _textFileService;

        public LatestResultController(ITextFileService textFileService)
        {
            _textFileService = textFileService;
        }

        [HttpGet]
        public async Task<ActionResult<int>> LatestNumber() 
        {
            return Ok(await _textFileService.ReadAsync());
        }
    }
}
