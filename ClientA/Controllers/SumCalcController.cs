using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;

namespace ClientA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SumCalcController : ControllerBase
    {

        private readonly ILogger<SumCalcController> _logger;
        private readonly GrpcChannel Channel;
        private readonly SumCalc.SumCalcClient Client;

        public SumCalcController(ILogger<SumCalcController> logger)
        {
            _logger = logger;
            Channel = GrpcChannel.ForAddress("https://localhost:7147");
            Client = new SumCalc.SumCalcClient(Channel);
        }

        [HttpPost]
        public async Task<IActionResult> SumTwoNumbers(int num1, int num2)
        {
            _logger.LogInformation("Received request to calculate sum of {Num1} and {Num2}", num1, num2);

            var response = await Client.CalculateSumAsync(new SumRequest
            {
                Num1 = num1,
                Num2 = num2
            });

            _logger.LogInformation("Calculated sum: {Result}", response);

            return Ok(response);


        }
    }
}
