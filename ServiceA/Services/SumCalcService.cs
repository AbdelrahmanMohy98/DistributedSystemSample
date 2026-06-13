using Grpc.Core;

namespace ServiceA.Services
{
    public class SumCalcService(ILogger<SumCalcService> logger, IRabbitMQService _rabbitmqService) : SumCalc.SumCalcBase
    {

        public override async Task<SumReply> CalculateSum(SumRequest request, ServerCallContext context)
        {
            logger.LogInformation("The sum is being calculated for {Num1} and {Num2}", request.Num1, request.Num2);

            double result = request.Num1 + request.Num2;

            await _rabbitmqService.PublishAsync(result);

            return await Task.FromResult(new SumReply
            {
                Result = result
            });
        }
    }
}
