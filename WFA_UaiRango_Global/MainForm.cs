using System.Timers;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WFA_UaiRango_Global.Dto;
using WFA_UaiRango_Global.Services.Culinaria;
using WFA_UaiRango_Global.Services.Estabelecimentos;
using WFA_UaiRango_Global.Services.Login;

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

        private DateTime _proximaExecucao;
        private readonly TimeSpan _intervaloExecucao = TimeSpan.FromMinutes(30);

        #region Inject services
        private readonly ILoginService _loginService;
        private readonly IEstabelecimentoService _estabelecimentoService;
        private readonly ICulinariaService _culinariaService;
        #endregion

        public MainForm(GlobalErpFiscalBaseContext db, ILogger<MainForm> logger,
            ILoginService loginService,
            IEstabelecimentoService estabelecimentoService,
            ICulinariaService culinariaService
            )
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

            #region Inject Services
            _loginService = loginService;
            _culinariaService = culinariaService;
            _estabelecimentoService = estabelecimentoService;
            #endregion

            _proximaExecucao = DateTime.Now; // ou DateTime.Now.AddMinutes(30)

            _timer = new System.Timers.Timer(1000); // ticks a cada 1 seg. (exemplo)
            _timer.Elapsed += TimerElapsed;
            _timer.Start();

            _uiTimer = new System.Windows.Forms.Timer();
            _uiTimer.Interval = 1000;
            _uiTimer.Tick += UiTimer_Tick;
            _uiTimer.Start();
        }

        private void AdicionarLinhaRichTextBox(string texto)
        {
            if (richTextBox1.InvokeRequired)
            {
                // se estamos em outra thread, chamamos Invoke no controle (UI)
                richTextBox1.Invoke(new Action(() => AdicionarLinhaRichTextBox(texto)));
            }
            else
            {
                // se estamos na thread da UI, podemos atualizar direto
                if (richTextBox1.Text.Length > 0)
                    richTextBox1.AppendText(Environment.NewLine + texto);
                else
                    richTextBox1.AppendText(texto);
            }
        }


        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_executando && DateTime.Now >= _proximaExecucao)
            {
                Task.Run(async () =>
                {
                    _executando = true;
                    _ultimaExecucao = DateTime.Now; // registra o momento em que começou

                    try
                    {
                        await UairangoIntegrarAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Erro na integração: {ex.Message}", ex);
                    }
                    finally
                    {
                        // terminou de executar, então configura o próximo horário
                        _executando = false;
                        _proximaExecucao = DateTime.Now.Add(_intervaloExecucao);
                    }
                });
            }
        }

        private void UiTimer_Tick(object sender, EventArgs e)
        {
            if (_executando)
            {
                var tempo = DateTime.Now - _ultimaExecucao;
                textBox1.ForeColor = Color.Green;
                textBox1.Text = $"Executando há: {tempo:hh\\:mm\\:ss}";
            }
            else
            {
                var restante = _proximaExecucao - DateTime.Now;
                if (restante < TimeSpan.Zero)
                    restante = TimeSpan.Zero;

                textBox1.ForeColor = Color.Red;
                textBox1.Text = $"Próxima execução em: {restante:hh\\:mm\\:ss}";
            }
        }

        #region Core
        private async Task UairangoIntegrarAsync()
        {
            var ultimoLogin = await _db.UairangoTokens
                .OrderByDescending(t => t.DataHoraGeracao)
                .FirstOrDefaultAsync();
            bool idadeDoTokenMenorQueUmDia =
                ultimoLogin != null
                && ultimoLogin.DataHoraGeracao.HasValue
                && (DateTime.Now - ultimoLogin.DataHoraGeracao.Value).TotalHours < 24;

            string? token = ultimoLogin?.TokenAcesso;
            if (ultimoLogin == null || idadeDoTokenMenorQueUmDia)
            {
                LoginUaiRangoResponseDto login = await this._loginService.LoginAsync();
                token = login.Token;

                UairangoToken uairangoToken = new UairangoToken
                {
                    TokenAcesso = token,
                    DataHoraGeracao = DateTime.Now
                };

                await _db.UairangoTokens.AddAsync(uairangoToken);
                await _db.SaveChangesAsync();
            }

            if (token != null)
            {
                AdicionarLinhaRichTextBox($"Logado com sucesso ({DateTime.Now})");
                await GetCulinarias(token);
            }
            else
            {
                _logger.LogError("Erro desconhecido ao fazer login no UaiRango");
            }
        }
        #region Services_Calls
        private async Task GetCulinarias(string token)
        {
            AdicionarLinhaRichTextBox($"Obtendo culinarias ({DateTime.Now})");
            try
            {
                var culinarias = await this._culinariaService.ObterCulinariasAsync(token);
                if (culinarias != null)
                {
                    foreach (var culinariaDto in culinarias)
                    {
                        var culinariaExistente = await _db.UairangoCulinarias
                            .FirstOrDefaultAsync(c => c.IdCulinariaUairango.Equals(culinariaDto.IdCulinaria.ToString()));

                        if (culinariaExistente != null)
                        {
                            culinariaExistente.NmCulinaria = culinariaDto.Nome;
                            culinariaExistente.MeioMeio = culinariaDto.MeioMeio;
                            _db.UairangoCulinarias.Update(culinariaExistente);
                        }
                        else
                        {
                            var novaCulinaria = new UairangoCulinaria
                            {
                                IdCulinariaUairango = culinariaDto.IdCulinaria.ToString(),
                                NmCulinaria = culinariaDto.Nome,
                                MeioMeio = culinariaDto.MeioMeio
                            };
                            await _db.UairangoCulinarias.AddAsync(novaCulinaria);
                        }
                    }

                    await _db.SaveChangesAsync();
                    AdicionarLinhaRichTextBox($"Culinarias obtidas ({DateTime.Now})");
                }
                else
                {
                    _logger.LogError("Erro desconhecido ao obter culinárias do UaiRango");
                    AdicionarLinhaRichTextBox($"Erro desconhecido ao obter culinárias do UaiRango ({DateTime.Now})");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter culinarias: {ex.Message}", ex);
                AdicionarLinhaRichTextBox($"Erro ao obter culinárias do UaiRango ({DateTime.Now}): {ex.Message}");
            }
        }

        #endregion
        #endregion
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
