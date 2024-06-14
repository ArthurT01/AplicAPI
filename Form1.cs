using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplicação_API
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void busca(string cep)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://viacep.com.br/ws/" + cep + "/json/");
            request.AllowAutoRedirect = false;
            HttpWebResponse ChecaServidor = (HttpWebResponse)request.GetResponse();

            if (ChecaServidor.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("Erro na requisição: " + ChecaServidor.StatusCode.ToString());
                return; // Encerra o códigoz
            }

            using (Stream webStream = ChecaServidor.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        String response = responseReader.ReadToEnd();
                        // MessageBox.Show(response);
                        response = Regex.Replace(response, "[{},]", string.Empty);
                        response = response.Replace("\"", "");
                        // MessageBox.Show(response);

                        String[] substrings = response.Split('\n');

                        int cont = 0;
                        foreach (var substring in substrings)
                        {
                            // CEP
                            if (cont == 1)
                            {
                                string[] valor = substring.Split(':');
                                txtCep.Text = valor[1].ToString();
                            }

                            // Logradouro
                            if (cont == 2)
                            {
                                string[] valor = substring.Split(':');
                                txtRua.Text = valor[1].ToString();
                            }


                            // Bairro
                            if (cont == 4)
                            {
                                string[] valor = substring.Split(':');
                                txtBairro.Text = valor[1].ToString();
                            }

                            // Cidade
                            if (cont == 5)
                            {
                                string[] valor = substring.Split(':');
                                txtCidade.Text = valor[1].ToString();
                            }

                            // UF
                            if (cont == 6)
                            {
                                string[] valor = substring.Split(':');
                                txtUf.Text = valor[1].ToString();
                            }
                            cont++;
                        }
                    }
                }
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            //executaBuscaBasica();
            //executaBusca();
            executaBuscaTratada();
        }

        private void executaBuscaBasica()
        {
            int c = Convert.ToInt32(txtCep.Text);

            BuscaCep BuscaCep = new BuscaCep(c);

            txtRua.Text = BuscaCep.Rua;
            txtBairro.Text = BuscaCep.Bairro;
            txtCidade.Text = BuscaCep.Cidade;
            txtUf.Text = BuscaCep.Estado;
        }

        private void executaBusca()
        {
            string entrada = txtCep.Text;
            int c;

            if(int.TryParse(entrada, out c))
            {
                BuscaCep BuscaCep = new BuscaCep(c);

                txtRua.Text = BuscaCep.Rua;
                txtBairro.Text = BuscaCep.Bairro;
                txtCidade.Text = BuscaCep.Cidade;
                txtUf.Text = BuscaCep.Estado;
            }
            else
            {
                MessageBox.Show("Por favor, insira um CEP válido que contenha apenas números.",
                                "Entrada Inválida",MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCep.Clear();
                txtCep.Focus();
            }
        }

        private void executaBuscaTratada()
        {
            string input = txtCep.Text;
            int c;

            if (int.TryParse(input, out c) && input.Length == 8)
            {
                try
                {
                    BuscaCep BuscaCep = new BuscaCep(c);

                    if(string.IsNullOrEmpty(BuscaCep.Cep))
                    {
                        MessageBox.Show("CEP não encontrado. " +
                                        "Verifique o número digitado e tente novamente.",
                                        "CEP Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        txtRua.Text = BuscaCep.Rua;
                        txtBairro.Text = BuscaCep.Bairro;
                        txtCidade.Text = BuscaCep.Cidade;
                        txtUf.Text = BuscaCep.Estado;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Ocorreu um erro ao buscar o CEP: " + ex.Message,
                                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                              
            }
            else
            {
                MessageBox.Show("Por favor, insira um CEP válido que contenha apenas números.",
                                "Entrada Inválida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCep.Clear();
                txtCep.Focus();
            }
        }
    }
}
