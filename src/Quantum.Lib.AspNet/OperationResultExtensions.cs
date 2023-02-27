using Microsoft.AspNetCore.Mvc;
using Quantum.Lib.Common;

namespace Quantum.Lib.AspNet
{
    public static class OperationResultExtensions
    {
        public static ActionResult ToActionResult(this OperationResult result)
        {
            if (result.IsNotFound)
            {
                return new NotFoundResult();
            }

            if (result.IsFailure)
            {
                return new BadRequestObjectResult(new ErrorResponse(result.Error));
            }

            return new NoContentResult();
        }

        public static ActionResult ToActionResult<T>(this OperationResult<T> result)
        {
            if (result.IsNotFound)
            {
                return new NotFoundResult();
            }

            if (result.IsFailure)
            {
                return new BadRequestObjectResult(new ErrorResponse(result.Error));
            }

            if (result.Value == null)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(result.Value);
        }
    }
}
