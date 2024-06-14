using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplicação_API
{
    internal class BuscaCep
    {
        public string Cep { get; set; }
        public string Rua { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Ibge { get; set; }
        public string Ddd { get; set; }
        public string Gia { get; set; }
        public string Siafi { get; set; }


        public BuscaCep(int c)
        {
            int cep = c;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://viacep.com.br/ws/" + cep + "/json/");
            request.AllowAutoRedirect = false;
            HttpWebResponse ChecaServidor = (HttpWebResponse)request.GetResponse();

            if (ChecaServidor.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("Erro na requisição: " + ChecaServidor.StatusCode.ToString());
                return; // Encerra o código
            }

            using (Stream webStream = ChecaServidor.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        String response = responseReader.ReadToEnd();
                        //MessageBox.Show(response);
                        response = Regex.Replace(response, "[{},]", string.Empty);
                        response = response.Replace("\"", "");
                        //MessageBox.Show(response);

                        String[] substrings = response.Split('\n');

                        int cont = 0;
                        foreach (var substring in substrings)
                        {
                            // CEP
                            if (cont == 1)
                            {
                                string[] valor = substring.Split(':');
                                Cep = valor[1].ToString();
                            }

                            // Logradouro
                            if (cont == 2)
                            {
                                string[] valor = substring.Split(':');
                                Rua = valor[1].ToString();
                            }

                            // Bairro
                            if (cont == 4)
                            {
                                string[] valor = substring.Split(':');
                                Bairro = valor[1].ToString();
                            }

                            // Cidade
                            if (cont == 5)
                            {
                                string[] valor = substring.Split(':');
                                Cidade = valor[1].ToString();
                            }

                            // UF
                            if (cont == 6)
                            {
                                string[] valor = substring.Split(':');
                                Estado = valor[1].ToString();
                            }

                            // IBGE
                            if (cont == 7)
                            {
                                string[] valor = substring.Split(':');
                                Ibge = valor[1].ToString();
                            }

                            // DDD
                            if (cont == 9)
                            {
                                string[] valor = substring.Split(':');
                                Ddd = valor[1].ToString();
                            }

                            // GIA
                            if (cont == 8)
                            {
                                string[] valor = substring.Split(':');
                                Gia = valor[1].ToString();
                            }

                            // SIAFI
                            if (cont == 10)
                            {
                                string[] valor = substring.Split(':');
                                Siafi = valor[1].ToString();
                            }
                            
                            cont++;

                        }
                    }
                }
            }
        }
    }
}
