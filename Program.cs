using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using PainelMesasLivres.Estrutura;

namespace PainelMesasLivres
{
    internal class Program
    {
        public static HttpListener listener;
        
        public static int pageViews = 0;
        public static int requestCount = 0;
       

        // https://cdn-icons-png.flaticon.com/512/1535/1535044.png
        //https://cdn-icons-png.flaticon.com/512/1535/1535041.png

        //https://cdn-icons-png.flaticon.com/512/6010/6010039.png

        public static async Task HandleIncomingConnections(string url)
        {
            bool runServer = true;

            Painel DisplayPainel = new Painel();
            Display oDisplay = new Display();
            


            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {


                DisplayPainel = oDisplay.Letreiro();

                string pageData =
                "<!DOCTYPE html>" +
                " <html lang=\"en\">" +
                " <head>" +
                " <title> Fila Senac Fast Food </title> " +
                $" <meta http-equiv=\"refresh\" content=\"15; URL={url}\">" +
                " <meta charset=\"utf-8\">" +
                " <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">" +
                " <link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/css/bootstrap.min.css\" rel=\"stylesheet\">\r\n  " +
                " <script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.2.1/dist/js/bootstrap.bundle.min.js\">" +
                " </script>" +
                " </head>" +
                " <body>" +
                " <div class=\"container mt-3\">" +
                " <table style=\"width:100%\"><tr><td><h4>  " +
                " <center>Disponiveis </center></h4></td><td>.</td>" +
                "  <td><h4><center>Reserva </center></h4></td><td>.</td> " +
                "  <td><h4> <center>Ocupação </center></h4></td><td>.</td>" +
                "  <td><h4> <center>Atendidos </center></h4></td></tr><tr>" +
                $" <td><h1 class=\"text-success\"><center>{DisplayPainel.Disponivel.ToString()} </center></h1></td>" +
                $" <td>.</td><td><h1 class=\"text-warning\"><center>{DisplayPainel.Reserva.ToString()}</center></h1></td>" +
                $" <td>.</td><td><h1 class=\"text-danger\"><center>{DisplayPainel.Ocupacao.ToString()}</center></h1></td>" +
                $" <td>.</td><td><h1 class=\"text-info\"><center>{DisplayPainel.Atendindos.ToString()}</center></h1></td></tr>" +
                $" <tr><td><center><img src=\"https://cdn-icons-png.flaticon.com/512/6009/6009640.png\" " +
                $" alt=\"Vago\" width=\"100\" height=\"100\"></center></td>" +
                $" <td>.</td><td><center><img src=\"https://cdn-icons-png.flaticon.com/512/2702/2702456.png\" " +
                $" alt=\"Reserva\" width=\"100\" height=\"100\"></center></td>" +
                $" <td>.</td><td><center><img src=\"https://cdn-icons-png.flaticon.com/512/6010/6010039.png\" " +
                $" alt=\"Ocupado\" width=\"100\" height=\"100\"></center></td>" +
                $" <td>.</td><td><center><img src=\"https://cdn-icons-png.flaticon.com/512/2839/2839174.png\" alt=\"Ocupado\" " +
                $" width=\"100\" height=\"100\"></center></td> </tr> </table> " +
                $" <table style=\"width:100%\"><tr><td>" +
                $" <div class=\"alert alert-success\"><strong><h5>Mesas Livre(s)</h5></strong></div></td><td>" +
                $" <div class=\"alert alert-dark\"><strong><h5>Mesas Ocupada(s)</h5></strong></div></td></tr>" +
                $" <tr><td></div> <div class=\"container mt-3\">" +
                $" {DisplayPainel.MesasLivres} " +
                $" </div> </td><td>" +
                $" <div class=\"container mt-3\"> " +
                $" {DisplayPainel.MesasOcupadas}" +
                $"</div></td></tr>" +
                $" <tr><td></td></tr> <tr><td></td></tr></table>" +
                $"<div class=\"container mt-3\"><h6>Fila</h6>" +
                $" <table class=\"table table-striped\"> <thead><tr><th>Numero</th><th>Cliente</th><th>Ocupação</th></tr></thead><tbody>" +
                $" {DisplayPainel.ListaEsperaFila}" +
                $" </table><h5>{DateTime.Now.ToString()}</h5></div>" +
                $"</body></html>";



                // Esperará aqui até ouvirmos uma conexão
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // Imprime algumas informações sobre a solicitação
                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();

                // Se a url `shutdown` for solicitada com POST, então desligue o servidor depois de servir a página
                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
                {
                    Console.WriteLine("Shutdown requested");
                    runServer = false;
                }

                // Certifique-se de não incrementar o contador de visualizações de página se `favicon.ico` for solicitado
                if (req.Url.AbsolutePath != "/favicon.ico")
                    pageViews += 1;

                // Write the response info
                string disableSubmit = !runServer ? "disabled" : "";
                byte[] data = Encoding.UTF8.GetBytes(pageData);
//                byte[] data = Encoding.UTF8.GetBytes(String.Format(pageData, pageViews, disableSubmit));

                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                //  resp.ContentLength64 = data.LongLength;

                // Gravar no fluxo de resposta (de forma assíncrona) e fechá-lo
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }

        static void Main(string[] args)
        {
            // Cria um servidor Http e começa a escutar as conexões de entrada

            String strHostName = string.Empty;
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ipEntry.AddressList;

            for (int i = 0; i < addr.Length; i++)
            {
                Console.WriteLine("IP Address {0}: {1} ", i, addr[i].ToString());
            }

           // Console.WriteLine("Qual Opção de IP?:");

          //  String Selecao = Console.ReadLine();

           // string server = addr[int.Parse(Selecao)].ToString();
            //server = Dns.GetHostName();
            //Console.WriteLine("Servidor  -->  {0}", server);

            //string url = $"http://{server}:8000/";
         //   string url = $"http://192.168.0.28:8000/";
            string url = $"http://localhost:8000/";
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Conexões on  {0}", url);

            // Trata requisições
            Task listenTask = HandleIncomingConnections(url);
            listenTask.GetAwaiter().GetResult();

            //Fecha o ouvinte
            listener.Close();
        }
    }
}
