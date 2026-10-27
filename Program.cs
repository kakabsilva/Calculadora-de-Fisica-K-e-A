namespace Kobayashi
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;

    namespace FisicaApp
    {
     
        public class Campo
        {
            public string Rotulo { get; }
            public string Unidade { get; }
            public string ValorPadrao { get; }

            public Campo(string rotulo, string unidade, string valorPadrao = "")
            {
                Rotulo = rotulo;
                Unidade = unidade;
                ValorPadrao = valorPadrao;
            }
        }

        
        public abstract class FormulaFisica
        {
            public string Titulo { get; protected set; }
            public string Equacao { get; protected set; }
            public string Descricao { get; protected set; }
            public string NomeResultado { get; protected set; }
            public string UnidadeResultado { get; protected set; }
            public Color Cor { get; protected set; }
            public List<Campo> Campos { get; } = new List<Campo>();

          
            public abstract double Calcular(double[] valores);
        }

        public class VelocidadeMedia : FormulaFisica
        {
            public VelocidadeMedia()
            {
                Titulo = "Velocidade Média";
                Equacao = "Vm = ΔS / Δt";
                Descricao = "Razão entre o deslocamento e o intervalo de tempo.";
                NomeResultado = "Vm";
                UnidadeResultado = "m/s";
                Cor = Color.FromArgb(52, 152, 219);
                Campos.Add(new Campo("Deslocamento ΔS", "m"));
                Campos.Add(new Campo("Intervalo de tempo Δt", "s"));
            }

            public override double Calcular(double[] v)
            {
                if (v[1] == 0) throw new ArgumentException("O tempo não pode ser zero.");
                return v[0] / v[1];
            }
        }

        public class Aceleracao : FormulaFisica
        {
            public Aceleracao()
            {
                Titulo = "Aceleração";
                Equacao = "a = ΔV / Δt";
                Descricao = "Variação da velocidade por unidade de tempo.";
                NomeResultado = "a";
                UnidadeResultado = "m/s²";
                Cor = Color.FromArgb(46, 204, 113);
                Campos.Add(new Campo("Variação de velocidade ΔV", "m/s"));
                Campos.Add(new Campo("Intervalo de tempo Δt", "s"));
            }

            public override double Calcular(double[] v)
            {
                if (v[1] == 0) throw new ArgumentException("O tempo não pode ser zero.");
                return v[0] / v[1];
            }
        }

        public class Forca : FormulaFisica
        {
            public Forca()
            {
                Titulo = "Força";
                Equacao = "F = m · a";
                Descricao = "2ª Lei de Newton: força resultante é massa vezes aceleração.";
                NomeResultado = "F";
                UnidadeResultado = "N";
                Cor = Color.FromArgb(231, 76, 60);
                Campos.Add(new Campo("Massa m", "kg"));
                Campos.Add(new Campo("Aceleração a", "m/s²"));
            }

            public override double Calcular(double[] v) => v[0] * v[1];
        }

        public class ForcaAtrito : FormulaFisica
        {
            public ForcaAtrito()
            {
                Titulo = "Força de Atrito";
                Equacao = "Fat = μ · N";
                Descricao = "Força de atrito é o coeficiente de atrito vezes a força normal.";
                NomeResultado = "Fat";
                UnidadeResultado = "N";
                Cor = Color.FromArgb(230, 126, 34);
                Campos.Add(new Campo("Coeficiente de atrito μ", "adim."));
                Campos.Add(new Campo("Força normal N", "N"));
            }

            public override double Calcular(double[] v)
            {
                if (v[0] < 0) throw new ArgumentException("O coeficiente de atrito não pode ser negativo.");
                return v[0] * v[1];
            }
        }

        public class Peso : FormulaFisica
        {
            public Peso()
            {
                Titulo = "Peso";
                Equacao = "P = m · g";
                Descricao = "Força com que a gravidade atrai um corpo.";
                NomeResultado = "P";
                UnidadeResultado = "N";
                Cor = Color.FromArgb(155, 89, 182);
                Campos.Add(new Campo("Massa m", "kg"));
                Campos.Add(new Campo("Gravidade g", "m/s²", "9,8"));
            }

            public override double Calcular(double[] v) => v[0] * v[1];
        }

        public class PrimeiraLeiOhm : FormulaFisica
        {
            public PrimeiraLeiOhm()
            {
                Titulo = "1ª Lei de Ohm";
                Equacao = "U = R · i";
                Descricao = "A tensão em um resistor é a resistência vezes a corrente.";
                NomeResultado = "U";
                UnidadeResultado = "V";
                Cor = Color.FromArgb(241, 196, 15);
                Campos.Add(new Campo("Resistência R", "Ω"));
                Campos.Add(new Campo("Corrente i", "A"));
            }

            public override double Calcular(double[] v) => v[0] * v[1];
        }

        public class MenuForm : Form
        {
            private const int Lado = 150;

            public MenuForm()
            {
                ConfigurarFormularioCelular(this, "Fórmulas de Física");

                var titulo = new Label
                {
                    Text = "Fórmulas de Física",
                    Font = new Font("Segoe UI", 18, FontStyle.Bold),
                    ForeColor = Color.White,
                    Dock = DockStyle.Top,
                    Height = 80,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                var painel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(10, 20, 0, 0),
                    FlowDirection = FlowDirection.LeftToRight,
                    WrapContents = true
                };

                var formulas = new List<FormulaFisica>
            {
                new VelocidadeMedia(), new Aceleracao(), new Forca(),
                new ForcaAtrito(), new Peso(), new PrimeiraLeiOhm()
            };

                int numero = 1;
                foreach (var f in formulas)
                {
                    painel.Controls.Add(CriarBotao(numero++, f));
                }

                Controls.Add(painel);
                Controls.Add(titulo);
            }

            private Button CriarBotao(int numero, FormulaFisica formula)
            {
                var btn = new Button
                {
                    Text = $"{numero}\n{formula.Titulo}\n\n{formula.Equacao}",
                    Size = new Size(Lado, Lado),
                    Margin = new Padding(8),
                    BackColor = formula.Cor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold)
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += (s, e) =>
                {
                    using (var tela = new FormulaForm(formula))
                    {
                        tela.ShowDialog(this);
                    }
                };
                return btn;
            }

            public static void ConfigurarFormularioCelular(Form form, string titulo)
            {
                form.Text = titulo;
                form.ClientSize = new Size(360, 640);   // proporção de celular
                form.FormBorderStyle = FormBorderStyle.FixedSingle;
                form.MaximizeBox = false;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.BackColor = Color.FromArgb(30, 36, 50);
            }
        }

        
        public class FormulaForm : Form
        {
            private readonly FormulaFisica _formula;
            private readonly List<TextBox> _entradas = new List<TextBox>();
            private readonly Label _lblResultado;

            public FormulaForm(FormulaFisica formula)
            {
                _formula = formula;
                MenuForm.ConfigurarFormularioCelular(this, formula.Titulo);

                var cabecalho = new Label
                {
                    Text = formula.Titulo,
                    Font = new Font("Segoe UI", 18, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = formula.Cor,
                    Dock = DockStyle.Top,
                    Height = 70,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                var painel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    Padding = new Padding(20, 15, 20, 10)
                };

                painel.Controls.Add(new Label
                {
                    Text = formula.Equacao,
                    Font = new Font("Segoe UI", 24, FontStyle.Bold),
                    ForeColor = formula.Cor,
                    Size = new Size(320, 50),
                    TextAlign = ContentAlignment.MiddleCenter
                });

                painel.Controls.Add(new Label
                {
                    Text = formula.Descricao,
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.LightGray,
                    Size = new Size(320, 45),
                    TextAlign = ContentAlignment.MiddleCenter
                });

                foreach (var campo in formula.Campos)
                {
                    painel.Controls.Add(new Label
                    {
                        Text = $"{campo.Rotulo} ({campo.Unidade})",
                        Font = new Font("Segoe UI", 10),
                        ForeColor = Color.White,
                        Size = new Size(320, 24)
                    });

                    var txt = new TextBox
                    {
                        Font = new Font("Segoe UI", 14),
                        Size = new Size(320, 34),
                        Text = campo.ValorPadrao,
                        Margin = new Padding(3, 0, 3, 12)
                    };
                    _entradas.Add(txt);
                    painel.Controls.Add(txt);
                }

                var btnCalcular = new Button
                {
                    Text = "CALCULAR",
                    Size = new Size(320, 50),
                    BackColor = formula.Cor,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold)
                };
                btnCalcular.FlatAppearance.BorderSize = 0;
                btnCalcular.Click += Calcular_Click;
                painel.Controls.Add(btnCalcular);

                _lblResultado = new Label
                {
                    Text = $"{formula.NomeResultado} = ?",
                    Font = new Font("Segoe UI", 16, FontStyle.Bold),
                    ForeColor = Color.White,
                    Size = new Size(320, 70),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                painel.Controls.Add(_lblResultado);

                var btnVoltar = new Button
                {
                    Text = "← VOLTAR",
                    Size = new Size(320, 45),
                    BackColor = Color.FromArgb(70, 78, 98),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold)
                };
                btnVoltar.FlatAppearance.BorderSize = 0;
                btnVoltar.Click += (s, e) => Close();
                painel.Controls.Add(btnVoltar);

                Controls.Add(painel);
                Controls.Add(cabecalho);
            }

            private void Calcular_Click(object sender, EventArgs e)
            {
                try
                {
                    var valores = new double[_entradas.Count];
                    for (int i = 0; i < _entradas.Count; i++)
                    {
                        string texto = _entradas[i].Text.Trim().Replace(',', '.');
                        if (!double.TryParse(texto, NumberStyles.Float, CultureInfo.InvariantCulture, out valores[i]))
                            throw new ArgumentException($"Valor inválido em \"{_formula.Campos[i].Rotulo}\".");
                    }

                    double r = _formula.Calcular(valores);
                    _lblResultado.ForeColor = Color.White;
                    _lblResultado.Text = $"{_formula.NomeResultado} = {r.ToString("0.##", CultureInfo.CurrentCulture)} {_formula.UnidadeResultado}";
                }
                catch (ArgumentException ex)
                {
                    _lblResultado.ForeColor = Color.Salmon;
                    _lblResultado.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    _lblResultado.Text = ex.Message;
                    return;
                }
                _lblResultado.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            }
        }

        static class Program
        {
            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MenuForm());
            }
        }
    }
}
