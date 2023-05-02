using System.Net;
using System.Net.Sockets;
using System.Text;

var serverAddress = IPAddress.Parse("127.0.0.1");
const int serverPort = 5000;

var server = new TcpListener(serverAddress, serverPort);
server.Start();
Console.WriteLine($"Server started on {serverAddress}:{serverPort}");

var i = 0;
while (true)
{
    var connectionNumber = Interlocked.Increment(ref i);

    var client = server.AcceptTcpClient();
    Console.WriteLine($"[{connectionNumber}] Connected to client at {client.Client.RemoteEndPoint}");

    var clientThread = new Thread(clientObj =>
    {
        if (clientObj == null)
        {
            return;
        }
        var connection = (TcpClient)clientObj;
        var stream = connection.GetStream();
        var buffer = new byte[1024];
        while (true)
        {
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead == 0)
            {
                Console.WriteLine($"[{connectionNumber}] Connection was closed by {connection.Client.RemoteEndPoint}");
                break;
            }
            if (Encoding.ASCII.GetString(buffer, 0, bytesRead) == "\\test\n")
            {
                Console.WriteLine($"[{connectionNumber}] Test command received. Closing connection to {connection.Client.RemoteEndPoint}");
                break;
            }
            var dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"[{connectionNumber}] Received {bytesRead} bytes from {connection.Client.RemoteEndPoint} at {dateTime}");
            stream.Write(Encoding.ASCII.GetBytes($"n={bytesRead}, t={dateTime}\n"));
        }
        stream.Close();
        connection.Close();
    });
    
    clientThread.Start(client);
}
