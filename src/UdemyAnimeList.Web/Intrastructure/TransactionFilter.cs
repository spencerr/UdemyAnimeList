using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using UdemyAnimeList.Data;

namespace UdemyAnimeList.Web.Infrastructure
{
    public class TransactionFilter : IAsyncActionFilter
    {
        private readonly ApplicationDbContext _context;

        public TransactionFilter(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var actionExecuted = await next();
                if (actionExecuted.Exception != null & !actionExecuted.ExceptionHandled)
                {
                    await transaction.RollbackAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
