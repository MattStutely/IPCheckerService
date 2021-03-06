using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace IPChecker.Services.ServiceAPI.Infrastructure
{
    public abstract class ExceptionHandler : IExceptionHandler
    {
        public virtual Task HandleAsync(ExceptionHandlerContext context,
                                        CancellationToken cancellationToken)
        {
            if (!ShouldHandle(context))
            {
                return Task.FromResult(0);
            }

            return HandleAsyncCore(context, cancellationToken);
        }

        public virtual Task HandleAsyncCore(ExceptionHandlerContext context,
                                           CancellationToken cancellationToken)
        {
            HandleCore(context);
            return Task.FromResult(0);
        }

        public virtual void HandleCore(ExceptionHandlerContext context)
        {
        }

        public virtual bool ShouldHandle(ExceptionHandlerContext context)
        {
            return context.ExceptionContext.CatchBlock.IsTopLevel;
        }
    } 
}
