using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;

class Program
{
    static void Main()
    {
        int Door = 5000;

        TcpListener listener = new TcpListener(IPAddress.Any, Door);
        //Inicia o receptor
        listener.Start();

        Console.WriteLine("O Servidor eniciou através da porta {0} \n", Door);

        //Loop para aceitar a nonexão contínua do cliente
        while (true)
        {
            //Mantém o programa bloqueado até que o cliente estabeleça conexão
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Conexão recebida da porta {0}", client.Client.RemoteEndPoint);

            //Obtem os dados que provem do cliente
            NetworkStream stream = client.GetStream();

            //Ler a mensagem enviada pelo cliente, armazena num buffer de bytes e converte em str
            byte[] buffer = new byte[1024];
            int bytes = stream.Read(buffer, 0, buffer.Length);
            string data = Encoding.UTF8.GetString(buffer, 0, bytes);
            
            int tamanho = data.Length;
            if (tamanho > 10)
            {
                Console.WriteLine("Mais de 10 digitos!!");
                Console.WriteLine("Numero gerado: {0} FIM.", data);
                Console.WriteLine("O numero é: {0} \n", Checks(tamanho));
                string Gerado = GenerateRandomNumber(tamanho);
                string Verification = Checks(tamanho);

                //**********RESPOSTA A SER ENVIADO PARA O CLIENTE**********
                byte[] imprime = Encoding.UTF8.GetBytes("Numero gerado: " + Gerado);
                stream.Write(imprime, 0, imprime.Length);
            }
            else
            {
                long valor = long.Parse(data);
                Console.WriteLine("Menos que 10 digitos!!");
                Console.WriteLine("Numero gerado: {0} FIM.", data);
                Console.WriteLine("O numero é: {0} \n", Checks(valor));

                string Gerado = GenerateRandomNumber(tamanho);
                string Verification = Checks(tamanho);

                //**********RESPOSTA A SER ENVIADO PARA O CLIENTE**********
                byte[] imprime = Encoding.UTF8.GetBytes("Numero gerado: " + Gerado + "\nO numero é: " + Verification + "\n");
                stream.Write(imprime, 0, imprime.Length);
            }

            stream.Close();
            client.Close();
        }
    }
    public static string GenerateRandomNumber(int tamanho)
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var result = new string(
            Enumerable.Repeat(chars, tamanho)
                      .Select(s => s[random.Next(s.Length)])
                      .ToArray());
        return result;
    }
    public static string Checks(long tamanho)
    {
        if(tamanho % 2 == 0) { return "Par"; }
        else { return "Impar"; }
    }
}