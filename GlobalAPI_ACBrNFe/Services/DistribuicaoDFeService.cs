
namespace GlobalAPI_ACBrNFe.Services
{
    public class DistribuicaoDFeService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly ILogger<DistribuicaoDFeService> _logger;
        public DistribuicaoDFeService(ILogger<DistribuicaoDFeService> logger)
        {
            _logger = logger;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }

        /// <summary>
        /// Método chamado quando o serviço é iniciado.
        /// </summary>
        /// <param name="cancellationToken">O token de cancelamento.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("DistribuicaoDFeService running.");
            var nextRunTime = GetNextRunTime();
            _logger.LogInformation("DistribuicaoDFeService start [" + nextRunTime.ToString() + "].");
            _timer = new Timer(DoWork, null, nextRunTime, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }
        /// <summary>
        /// Obtém o próximo horário de execução do serviço.
        /// </summary>
        /// <returns>O horário de execução do serviço.</returns>
        private static TimeSpan GetNextRunTime()
        {
            var horaAtual = DateTime.Now;
            var horaMarcada = new DateTime(horaAtual.Year, horaAtual.Month, horaAtual.Day, 11, 00, 10);

            if (horaAtual > horaMarcada)
            {
                horaMarcada = horaMarcada.AddDays(1);
            }

            return horaMarcada - horaAtual;
        }

        /// <summary>
        /// Método que realiza o trabalho principal do serviço.
        /// </summary>
        /// <param name="state">O estado do temporizador.</param>
        private void DoWork(object state)
        {
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Atualizador de Token pausando execução.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
