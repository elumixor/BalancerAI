import socket
import socketserver
import struct


class TorchTCPHandler(socketserver.BaseRequestHandler):
    def receive_vector(self):
        size = struct.unpack('<i', self.request.recv(4))[0]
        return list(struct.unpack(f'<{size}f', self.request.recv(size * 4)))

    def receive_float(self):
        return struct.unpack('<f', self.request.recv(4))[0]

    def receive_int(self):
        return struct.unpack('<i', self.request.recv(4))[0]

    def receive_vector_array(self):
        number_of_elements = struct.unpack('<i', self.request.recv(4))[0]
        return [self.receive_vector() for _ in range(number_of_elements)]


def start_unity_handler(request_handler, port=11000):
    server = socketserver.TCPServer((socket.gethostname(), port), request_handler)
    print("Server started")
    server.serve_forever()
