using Lib.Interfaces;
using Lib.Models;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
public class MeterReadingUploadsController : ControllerBase
{
    private readonly ILogger<MeterReadingUploadsController> _logger;
    private readonly ICsvParser<MeterReading> csvParser;
    private readonly IMeterReadingService meterReadingService;

    public MeterReadingUploadsController(ILogger<MeterReadingUploadsController> logger, ICsvParser<MeterReading> csvParser, IMeterReadingService meterReadingService)
    {
        _logger = logger;
        this.csvParser = csvParser;
        this.meterReadingService = meterReadingService;
    }

    [HttpPost]
    [Route("meter-reading-uploads")]
    public async Task<IActionResult> UploadMeterReadings(IFormFile file)
    {
        using var file1 = Request.Form.Files.FirstOrDefault()?.OpenReadStream();
        if (file1 == null) return BadRequest();

        var items = csvParser.GetItems(file1);
        var result = await meterReadingService.ProcessMeterReadings(items);

        return Ok(result);
    }
}
