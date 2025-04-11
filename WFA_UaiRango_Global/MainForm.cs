using System.Timers;
using GlobalErpData.Data;
using GlobalErpData.Models;
using Microsoft.Extensions.Logging;

namespace WFA_UaiRango_Global
{
    public partial class MainForm : Form
    {
        private System.Timers.Timer _timer;
        private System.Windows.Forms.Timer _uiTimer; // para atualizar o textBox1
        private DateTime _ultimaExecucao;
        private bool _executando = false;
        private readonly GlobalErpFiscalBaseContext _db;
        private readonly ILogger<MainForm> _logger;

        public MainForm(GlobalErpFiscalBaseContext db, ILogger<MainForm> logger)
        {
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            _logger = logger;
            _db = db;
            _logger.LogInformation("MainForm iniciado com sucesso.");
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;

            notifyIcon1.Visible = true;

            _ultimaExecucao = DateTime.Now;

            _timer = new System.Timers.Timer(20 * 60 * 1000); // 20 minutos
            _timer.Elapsed += TimerElapsed;
            _timer.Start();

            // Timer para atualizar o textBox1 a cada 1 segundo
            _uiTimer = new System.Windows.Forms.Timer();
            _uiTimer.Interval = 1000; // 1 segundo
            _uiTimer.Tick += UiTimer_Tick;
            _uiTimer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Task.Run(async () =>
            {
                _executando = true;
                _ultimaExecucao = DateTime.Now;

                try
                {
                    await IntegrarComIFoodAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro na integração: {ex.Message}");
                }

                _executando = false;
            });
        }

        private void UiTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan tempo;

            if (_executando)
            {
                tempo = DateTime.Now - _ultimaExecucao;
                textBox1.ForeColor = Color.Green;
                textBox1.Text = $"Executando há: {tempo:hh\\:mm\\:ss}";
            }
            else
            {
                tempo = (_ultimaExecucao.AddMinutes(20)) - DateTime.Now;

                if (tempo.TotalSeconds < 0)
                    tempo = TimeSpan.Zero;

                textBox1.ForeColor = Color.Red;
                textBox1.Text = $"Próxima execução em: {tempo:hh\\:mm\\:ss}";
            }
        }


        private async Task IntegrarComIFoodAsync()
        {
            // Simula delay da chamada HTTP
            await Task.Delay(1000);

            // Aqui você faz a lógica real: fetch, gravação no banco via EF, etc.
            Console.WriteLine("Integrando com iFood em segundo plano...");
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    // Restaura a janela
            //    this.Show();
            //    this.WindowState = FormWindowState.Normal;
            //    this.ShowInTaskbar = true;
            //}
        }

        private void buttonMinimizar_Click(object sender, EventArgs e)
        {
            // Esconde a janela, mas mantém o app rodando na bandeja
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void buttonFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var resultado = MessageBox.Show("Tem certeza que deseja sair?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                _timer?.Stop();
                _timer?.Dispose();
                notifyIcon1.Visible = false;
                // segue o fechamento normalmente
            }
            else
            {
                e.Cancel = true; // impede o fechamento
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.Visible && this.WindowState != FormWindowState.Minimized)
                {
                    // Minimiza e esconde
                    this.WindowState = FormWindowState.Minimized;
                    this.ShowInTaskbar = false;
                    this.Hide();
                }
                else
                {
                    // Restaura e mostra
                    this.StartPosition = FormStartPosition.CenterScreen; // só se quiser garantir que fique no centro
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    this.ShowInTaskbar = true;
                }
            }
        }

    }
}
