using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Communication {
    public class TorchCommunicator {
        // The port number for the remote device.  
        private readonly int port;
        // private readonly TcpClient tcpClient;
        private readonly string ipAddress;
        // private readonly Stream stm;

        public TorchCommunicator(int port) {
            this.port = port;
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[1].ToString();
        }

        public async Task<byte[]> Send(byte[] data) {
            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(ipAddress, port);
            var stm = tcpClient.GetStream();
            
            await stm.WriteAsync(data, 0, data.Length);
            
            var bb = new byte[1024];
            await stm.ReadAsync(bb, 0, 1024);
            
            tcpClient.Close();
            return bb;
        }

        ~TorchCommunicator() {
        }
    }
}