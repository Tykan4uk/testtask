using Microsoft.AspNetCore.Mvc.Filters;

namespace Api
{
    public class ExceptionInterceptor : ExceptionFilterAttribute, IAsyncExceptionFilter
    {
        public ExceptionInterceptor() {}

        public override Task OnExceptionAsync(ExceptionContext context) => Task.CompletedTask;
    }
}
