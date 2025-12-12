using System.Text.Json;
using System.Text;
namespace RepliDonaCarne
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer _timer;
        private NotifyIcon notifyIcon;
        private string apiUrlBase;
        private string caminhoArquivoLock;
        private bool _isCallingApi = false;
        private static readonly HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(15) // tempo máximo de espera
        };
        public Form1()
        {
            InitializeComponent();
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            var conf = LerConfiguracao("Config.conf");
            apiUrlBase = conf.GetValueOrDefault("API_URL")?.TrimEnd('/');

            int segundos = 60;
            if (!int.TryParse(conf.GetValueOrDefault("segundosRepli"), out segundos) || segundos <= 0)
                segundos = 60;

            caminhoArquivoLock = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "envio_inicial.txt");

            AtualizarEstadoBotaoPorArquivo();
            StartTimer(segundos);
            InitializeNotifyIcon();

            // Só iniciar replicação automática se a carga inicial já foi feita
            if (File.Exists(caminhoArquivoLock))
            {
                if (!_isCallingApi)
                    await Task.Run(() => ReplicarTudo());
            }
            else
            {
                AdicionarMensagemResponse(
                    "Carga inicial ainda não foi realizada. A replicação automática está desativada.",
                    "Aviso"
                );
            }
        }

        private void AtualizarEstadoBotaoPorArquivo()
        {
            if (File.Exists(caminhoArquivoLock))
                buttonEnvioInicial.Enabled = false;
            else
                buttonEnvioInicial.Enabled = true;
        }
        private void _timer_Tick(object sender, EventArgs e)
        {
            AtualizarEstadoBotaoPorArquivo();
        }
        private void InitializeNotifyIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new System.Drawing.Icon("images.ico");
            notifyIcon.Text = "Replicador Dona Carne";
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
        }
        private Dictionary<string, string> LerConfiguracao(string caminhoArquivo)
        {
            var config = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var linha in File.ReadAllLines(caminhoArquivo))
            {
                if (string.IsNullOrWhiteSpace(linha) || linha.Trim().StartsWith("#") || !linha.Contains("="))
                    continue;

                var partes = linha.Split(new[] { '=' }, 2);
                var chave = partes[0].Trim();
                var valor = partes[1].Trim();
                config[chave] = valor;
            }
            return config;
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.ShowBalloonTip(1000, "Replicador", "Rodando em segundo plano", ToolTipIcon.Info);
            }
        }
        private void TimerElapsed(object sender, EventArgs e)
        {
            _timer.Stop();
            try
            {
                AtualizarEstadoBotaoPorArquivo();

                if (File.Exists(caminhoArquivoLock))
                {
                    ReplicarTudo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _timer.Start();
            }
        }
        private void StartTimer(int segundos)
        {
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = segundos * 1000;
            _timer.Tick += TimerElapsed;
            _timer.Start();
        }
        private async void ReplicarTudo()
        {
            if (_isCallingApi) return;
            _isCallingApi = true;
            try
            {
                var requestUrl = $"{apiUrlBase}/api/Replicador/replicar";
                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                {
                    Content = new StringContent(string.Empty, Encoding.UTF8, "application/json")
                };
                var response = await client.SendAsync(request);
                var jsonString = await response.Content.ReadAsStringAsync();

                // Exibir a resposta completa no textBox
                SubstituirMensagemResponse(jsonString, "Resposta Completa");

                if (response.IsSuccessStatusCode)
                {
                    var resultado = JsonSerializer.Deserialize<RespostaApi>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (resultado == null || !resultado.Sucesso)
                    {
                        AdicionarMensagemResponse("A API respondeu, mas não teve sucesso.", "Erro");
                    }
                }
                else
                {
                    AdicionarMensagemResponse($"Erro HTTP: {response.StatusCode}", "Erro");
                }
            }
            catch (Exception ex)
            {
                AdicionarMensagemResponse($"Erro na replicação: {ex.Message}", "Erro");
            }
            finally
            {
                _isCallingApi = false;
            }
        }
        private void AdicionarMensagemResponse(string mensagem, string titulo = null)
        {
            string textoFinal = titulo != null ? $"[{titulo}] {mensagem}" : mensagem;
            // Mostra a mensagem no TextBox
            if (textBoxResponse.InvokeRequired)
            {
                textBoxResponse.Invoke(new Action(() =>
                {
                    textBoxResponse.AppendText(textoFinal + Environment.NewLine);
                }));
            }
            else
            {
                textBoxResponse.AppendText(textoFinal + Environment.NewLine);
            }
            if (textoFinal.Contains("Erro", StringComparison.OrdinalIgnoreCase))
            {
                GravarLogErro(textoFinal);
            }
        }
        private void GravarLogErro(string mensagemErro)
        {
            try
            {
                string caminhoLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log_erro.txt");
                List<string> linhasValidas = new List<string>();
                DateTime agora = DateTime.Now;

                if (File.Exists(caminhoLog))
                {
                    var linhas = File.ReadAllLines(caminhoLog);
                    foreach (var linha in linhas)
                    {
                        if (linha.Length >= 19) 
                        {
                            string dataStr = linha.Substring(0, 19);
                            if (DateTime.TryParseExact(dataStr, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime dataLinha))
                            {
                                if ((agora - dataLinha).TotalDays <= 7)
                                {
                                    linhasValidas.Add(linha);
                                }
                            }
                        }
                    }
                }
                string novaLinha = $"{agora:yyyy-MM-dd HH:mm:ss} - {mensagemErro}";
                linhasValidas.Add(novaLinha);
                File.WriteAllLines(caminhoLog, linhasValidas);
            }
            catch (Exception ex)
            {
                textBoxResponse.AppendText($"[LOG] Falha ao gravar erro no log: {ex.Message}{Environment.NewLine}");
            }
        }
        private void SubstituirMensagemResponse(string mensagem, string titulo = null)
        {
            string textoFinal = titulo != null ? $"[{titulo}] {mensagem}" : mensagem;
            if (textBoxResponse.InvokeRequired)
            {
                textBoxResponse.Invoke(new Action(() =>
                {
                    textBoxResponse.Clear(); // Limpa tudo
                    textBoxResponse.Text = textoFinal + Environment.NewLine;
                }));
            }
            else
            {
                textBoxResponse.Clear(); // Limpa tudo
                textBoxResponse.Text = textoFinal + Environment.NewLine;
            }
        }
        private void textBoxResponse_TextChanged(object sender, EventArgs e)
        {}
        private async void ButtonEnvioInicial_Click(object sender, EventArgs e)
        {
            if (_isCallingApi)
            {
                AdicionarMensagemResponse("Uma chamada já está em andamento. Aguarde finalizar.", "Aviso");
                return;
            }

            if (File.Exists(caminhoArquivoLock))
            {
                AdicionarMensagemResponse("Envio inicial já foi realizado. Botão desativado.", "Aviso");
                return;
            }

            _isCallingApi = true;
            _timer.Stop();
            AdicionarMensagemResponse("Iniciando envio inicial...", "Info");
            try
            {
                File.WriteAllText(caminhoArquivoLock, "Replicação inicial iniciada");
                var requestUrl = $"{apiUrlBase}/api/Replicador/replicarTodos";
                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                {
                    Content = new StringContent(string.Empty, Encoding.UTF8, "application/json")
                };
                var response = await client.SendAsync(request);
                var jsonString = await response.Content.ReadAsStringAsync();

                SubstituirMensagemResponse(jsonString, "Resposta Completa");

                if (response.IsSuccessStatusCode)
                {
                    var resultado = JsonSerializer.Deserialize<RespostaApi>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (resultado == null || !resultado.Sucesso)
                    {
                        AdicionarMensagemResponse("A API respondeu, mas não teve sucesso.", "Erro");
                    }
                    else
                    {
                        AdicionarMensagemResponse("Replicação finalizada com sucesso!", "Final");
                    }
                }
                else
                {
                    AdicionarMensagemResponse($"Erro HTTP: {response.StatusCode}", "Erro");
                }
                AtualizarEstadoBotaoPorArquivo();
            }
            catch (Exception ex)
            {
                AdicionarMensagemResponse($"Erro no envio inicial: {ex.Message}", "Erro");
            }
            finally
            {
                _isCallingApi = false;
                _timer.Start();
            }
        }
    }
}
