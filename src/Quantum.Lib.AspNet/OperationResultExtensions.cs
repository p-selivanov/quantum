using Microsoft.AspNetCore.Mvc;
using Quantum.Lib.Common;

namespace Quantum.Lib.AspNet;

public static class OperationResultExtensions
{
    public static ActionResult ToActionResult(this OperationResult result)
    {
        if (result.IsFailure)
        {
            if (result.Error.Code == "404")
            {
                return new NotFoundResult();
            }

            return new BadRequestObjectResult(new ErrorResponse(result.Error.Message, result.Error.Code));
        }

        return new NoContentResult();
    }

    public static ActionResult ToActionResult<T>(this OperationResult<T> result)
    {
        if (result.IsFailure)
        {
            if (result.Error.Code == "404")
            {
                return new NotFoundResult();
            }

            return new BadRequestObjectResult(new ErrorResponse(result.Error.Message, result.Error.Code));
        }

        if (result.Value is null)
        {
            return new NoContentResult();
        }

        return new OkObjectResult(result.Value);
    }
}
