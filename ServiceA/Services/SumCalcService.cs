using Grpc.Core;

namespace ServiceA.Services
{
    public class SumCalcService(ILogger<SumCalcService> logger) : SumCalc.SumCalcBase
    {
        public override Task<SumReply> CalculateSum(SumRequest request, ServerCallContext context)
        {
            logger.LogInformation("The sum is being calculated for {Num1} and {Num2}", request.Num1, request.Num2);

            return Task.FromResult(new SumReply
            {
                Result = request.Num1 + request.Num2
            });
        }
    }
}
