

using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Enfile.Presentation.Filter
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Result= new BadRequestObjectResult(
                    actionContext.ModelState.Values
                        .SelectMany(e => e.Errors)
                        .Select(e => e.ErrorMessage)
                );

                return;
            }

            base.OnActionExecuting(actionContext);
        }
    }
}