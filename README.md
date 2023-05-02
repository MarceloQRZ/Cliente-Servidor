# Cliente-Servidor
Programa Cliente/Servidor simples desenvolvido em C#.

## Metas
1. Introduzir a interface de programação de sockets.
2. Introduzir os conceitos de aplicação cliente e servidor.

### 👉 Pré-requisitos
Antes de começar, você precisará ter as seguintes ferramentas instaladas em sua máquina:
[Git](https://git-scm.com), [.NET](https://dotnet.microsoft.com/en-us/download).

Além disso, é bom ter um editor para trabalhar com códigos como [VSCode](https://code.visualstudio.com/), ou pode ser uma IDE [Visual Studio Code](https://visualstudio.microsoft.com/pt-br/downloads/).

### 🎲 Executando o Back End (servidor)

```bash
# Clonando o repositório
$ git clone <https://github.com/MarceloQRZ/Cliente-Servidor>

# Acessando a pasta do projeto pelo terminal/cmd
$ cd Cliente-Servidor

# Para rodar via terminal o programa Servidor
$ cd Server-Client
$ dotnet run

# Após iniciar o Servidor, em outra janela do terminal rodar o programa Cliente
$ cd Client/Client
$ dotnet run
```

## 👉 Implementação
1. Programa Cliente
- [x] Se conectar ao Servidor;
- [x] Gerar número aleatório inteiro com até 30 casas;
- [x] Enviar o número para o Servidor;
- [x] Receber, imprimir no console e devolver o valor recebido do servidor + “FIM”;
- [x] Fechar a conexão;
- [x] Repetir a cada 10 segundos;

2. Programa Servidor
- [x] Aguardar a conexão do cliente;
- [x] Receber o número;
- [x] Verificar o tamanho do número;
- [x] Se tiver mais de 10 casas, gerar e enviar uma string do mesmo tamanho para o cliente;
- [x] Se for menor que 10, verificar se é ímpar ou par e enviar o resultado da verificação ao cliente;
  
## 👉 Descrição da Implementação
### 1. Programa Cliente

```c#
- Conexão com o Servidor
// configurações de Ip e Porta já estabelecidas manualmente para o Cliente
string Ip = "127.0.0.1";
int Door = 5000;

// cria uma nova instância da classe 'TcpClient' e estabele uma conexão TCP com o servidor em uma determinada porta
TcpClient client = new TcpClient(Ip, Door);
```

```c#
- Gerar número aleatório
// A função Generate é responsável por gerar o número aleatório
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

// cria um array de bytes contendo a representação em UTF-8 de uma string gerada pela função Generate()
byte[] data = Encoding.UTF8.GetBytes(Generate());

/*
- Encoding.UTF8: É uma propriedade estática da classe Encoding em C# que retorna uma instância da classe UTF8Encoding, 
que é uma classe que codifica caracteres Unicode em sequências de bytes usando o formato UTF-8. O UTF-8 é um formato de 
codificação de caracteres que pode representar qualquer caractere Unicode usando um ou mais bytes.
- GetBytes(): É um método da classe Encoding em C# que converte uma string em uma matriz de bytes, usando a codificação 
especificada. Nesse caso, estamos usando a codificação UTF-8, que converterá a string em uma sequência de bytes.
*/
```

```c#
- Enviar o número para o Servidor

/*
Fornece uma maneira fácil de enviar e receber dados por uma conexão de rede, como uma conexão de soquete TCP ou UDP. 
O método GetStream() é usado em uma instância da classe TcpClient para obter o fluxo de rede associado a essa conexão.
*/
NetworkStream stream = client.GetStream();

/*
Envia uma matriz de bytes para o servidor através do objeto NetworkStream associado à conexão TCP.
Parâmetros -> (array, índice inicial, tamanho dos dados)
*/
stream.Write(data, 0, data.Length);
```

```c#
- Receber, imprimir no console e devolver o valor recebido do servidor + “FIM”

// Cria um novo array de bytes com um tamanho de 1024 bytes. Esse array será usado para armazenar a resposta do servidor.
data = new byte[1024];

// Lê os dados recebidos do servidor através do objeto NetworkStream
int bytes = stream.Read(data, 0, data.Length);

// Converte a matriz de bytes recebida do servidor em uma string, usando a codificação UTF-8
string response = Encoding.UTF8.GetString(data, 0, bytes);

// Imprime na tela a resposta do servidor
Console.WriteLine("\nRESPOSTA DO SERVIDOR: \n{0} ", response);
```
```c#
- Fechar a conexão

//  Fecha o objeto NetworkStream associado à conexão TCP com o servidor.
stream.Close();

//  Fecha o objeto TcpClient que foi usado para estabelecer a conexão TCP com o servidor.
client.Close();
```

### 2. Programa Servidor
```c#
- Conexão com o Cliente
// configurações de Porta já estabelecidas manualmente para o Cliente
int Door = 5000;

// aguarda a conexão de um cliente em uma porta específica.
TcpListener listener = new TcpListener(IPAddress.Any, Door);

/*
IPAddress.Any: É um valor especial que representa todas as interfaces de rede disponíveis no sistema. 
Isso significa que o servidor estará ouvindo em todas as interfaces de rede do sistema.
*/

// aguardar conexões de entrada de clientes
listener.Start();

// aguarda a chegada de uma conexão de entrada de um cliente e aceita a conexão assim que ela chega.
TcpClient client = listener.AcceptTcpClient();

// Mostrar que se conectou com sucesso
Console.WriteLine("Conexão recebida da porta {0}", client.Client.RemoteEndPoint);
```

```c#
- Receber o número

//Obtém os dados que provém do cliente
NetworkStream stream = client.GetStream();

/*
Cria um objeto NetworkStream que permite que o servidor leia e escreva dados na conexão com o cliente. 
Isso permite que o servidor envie e receba mensagens para e do cliente.
*/
```

```c#
- Verificar o tamanho do número

/*
Cria um array de bytes chamado buffer com tamanho 1024. Ele é usado para armazenar os dados 
lidos da stream de dados da conexão com o cliente.
*/
byte[] buffer = new byte[1024];

/*
Lê os dados da stream de dados da conexão com o cliente e armazena-os no 
buffer. O método Read retorna o número de bytes lidos.
*/
int bytes = stream.Read(buffer, 0, buffer.Length);

/*
Converte os dados lidos em uma string usando a codificação UTF-8. 
O método GetString decodifica um array de bytes em uma string. 
O terceiro parâmetro especifica o número de bytes a serem decodificados.
*/
string data = Encoding.UTF8.GetString(buffer, 0, bytes);

// armazena o tamanho
int tamanho = data.Length;
```

```c#
- Restrições do tamanho do número

// Funcão que gera a sting aleatória do mesmo tamanho do número para mostrar ao cliente
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

// Funcão para retornar se é par ou ímpar
public static string Checks(long tamanho)
    {
        if(tamanho % 2 == 0) { return "Par"; }
        else { return "Impar"; }
    }

// Verificação pelo tamanho
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
```

```c#
- Fechar a conexão

//  Fecha o objeto NetworkStream associado à conexão TCP com o servidor.
stream.Close();

//  Fecha o objeto TcpClient que foi usado para estabelecer a conexão TCP com o servidor.
client.Close();
```
