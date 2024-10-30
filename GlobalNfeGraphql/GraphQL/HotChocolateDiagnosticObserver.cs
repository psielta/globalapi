using HotChocolate.Execution.Instrumentation;
using HotChocolate.Execution;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;

namespace GlobalNfeGraphql.GraphQL
{
    public class HotChocolateDiagnosticObserver : ExecutionDiagnosticEventListener
    {
        private readonly ILogger<HotChocolateDiagnosticObserver> _logger;

        public HotChocolateDiagnosticObserver(ILogger<HotChocolateDiagnosticObserver> logger)
        {
            _logger = logger;
        }

        public override void RequestError(IRequestContext context, Exception exception)
        {
            _logger.LogError(exception, "Erro durante a execução da requisição GraphQL.");
        }

        public override void ResolverError(IMiddlewareContext context, IError error)
        {
            _logger.LogError("Erro no resolver {Path}: {ErrorMessage}", context.Path, error.Message);
        }

        public override void SubscriptionEventError(SubscriptionEventContext context, Exception exception)
        {
            _logger.LogError(exception, "Erro no evento de subscrição GraphQL.");
        }
    }

}
