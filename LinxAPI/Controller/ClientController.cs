using LinxAPI.Entity;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LinxAPI
{
    class ClientIndex
    {
        static async Task Main(string[] args)
        {
            SqlCommand comn = new SqlCommand();
            comn.Connection = LogEntity.abrir(); 

            HttpClient client;
            client = new HttpClient();
            client.BaseAddress = new Uri("https://hfllinxintegracaogiftwebapi-hom.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.Write("Digite o numero do seu Documento: ");
            var numDoc = Console.ReadLine(); //armazena o documento do usuario em uma variavel para ser usado para conexão com API
            
            Console.WriteLine("Processando e vericando registros...");

            object data = new
            {
                chaveIntegracao = "4B335B6F-9C4D-47F7-B798-C46FFBC4881A",
                codigoLoja = "1",
                numeroCartao = numDoc, //32231126850
                nsuCliente = "Gerar um Guid para cada solicitação",
                codigoSeguranca = "1234"
            };

            var json = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/LinxServiceApi/FidelidadeService/ConsultaFidelizacao", json);
            string result = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<ClientEntity>(result);
            //comn.CommandText = "insert into log (RetornoAPI) values (" + user.saldoEmReais + ", "+user.saldoEmPontos+")";
            //comn.ExecuteNonQuery();

            if (user.mensagemErro == "")
            {
                Console.WriteLine("Seu saldo em Reais é de: " + user.saldoEmReais + " e seu saldo de pontos é de: " + user.saldoEmPontos);
                comn.CommandText = "insert into log (CodUsuario, RetornoAPI, MensagemErro, DataHora) values (" + numDoc + ", 'Solicitação de Saldo e Pontos', 'OK', GETDATE())";
                comn.ExecuteNonQuery();
            }
            else
            {
                Console.WriteLine(user.mensagemErro);
                comn.CommandText = "insert into log (CodUsuario, MensagemErro, DataHora) values (" + numDoc + ", 'ERRO', GETDATE())";
                comn.ExecuteNonQuery();
            }

            comn.Connection.Close();
            Console.ReadKey();
        }
    }
}
