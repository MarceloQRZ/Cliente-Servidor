using System;
using System.Timers;
using System.Net.Sockets;
using System.Text;
using System.IO;

class Program
{
    static void Main()
    {
        string Ip = "127.0.0.1";
        int Door = 5000;

        //instancia uma conexão com o cliente (IP) na atravez da poarta (Door)
        TcpClient client = new TcpClient(Ip, Door);


        //
        NetworkStream stream = client.GetStream();

        var time = new System.Timers.Timer(10000);


        byte[] data = Encoding.UTF8.GetBytes(Generate());
        //Envia a mensagem para o servidor com os parametros (array, indice inicial, tamanho dos dados)
        stream.Write(data, 0, data.Length);
        
        //é criado um buffer de bytes para armazenar a resposta do servidor
        data = new byte[1024];
        int bytes = stream.Read(data, 0, data.Length);
        string response = Encoding.UTF8.GetString(data, 0, bytes);
        Console.WriteLine("RESPOSTA DO SERVIDOR: \n{0} ", response);

        stream.Close();
        client.Close();




    }
    public static string Generate()
    {
        Random random = new Random();
        int numDigits = random.Next(1, 31); // Gera um número aleatório entre 1 e 30 (inclusive)
        string randomNumber = "";
        for (int i = 0; i < numDigits; i++)
        {
            randomNumber += random.Next(0, 10).ToString(); // Gera um dígito aleatório de 0 a 9 e adiciona à string
        }
        string message = randomNumber;

        return message;
    }

}