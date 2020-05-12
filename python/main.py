import socketserver
import socket
import struct

import numpy as np
import torch
from torch.nn import Linear
from torch.optim import Adam, SGD


def list_to_bytes(float_list):
    return struct.pack(f'<i', len(float_list)) + struct.pack(f'<{len(float_list)}f', *float_list)


def tensor_to_bytes(tensor):
    shape = list_to_bytes(list(tensor.shape))
    data = list_to_bytes(list(tensor.flatten().tolist()))
    return shape + data


def list_from_bytes(bytes_list):
    l = struct.unpack('<i', bytes_list[0:4])[0]
    return struct.unpack(f'<{l}f', bytes_list[4:(4 + l * 4)])


def tensor_from_bytes(bytes):
    shape = [int(s) for s in list_from_bytes(bytes)]
    offset = 4 + len(shape) * 4
    data = list_from_bytes(bytes[offset:4 + offset + int(np.prod(shape)) * 4])

    return torch.tensor(data).reshape(shape)


def net_to_bytes(net):
    # return tensor_to_bytes(net.l1.weight) \
    #        + tensor_to_bytes(net.l1.bias) \
    #        + tensor_to_bytes(net.l2.weight) \
    #        + tensor_to_bytes(net.l2.bias) \
    #        + tensor_to_bytes(net.l3.weight) \
    #        + tensor_to_bytes(net.l3.bias)
    return tensor_to_bytes(net.weight) + tensor_to_bytes(net.bias)


discounting = 0.99


def to_discounted(rewards):
    r_last = 0

    for i in range(len(rewards) - 1, -1, -1):
        r_last = rewards[i] = rewards[i] + discounting * r_last


epoch = 1


def train_net(states, actions, rewards):
    global epoch
    print(f'Starting training')
    # print(states)

    p0 = torch.sigmoid(net(states)).flatten()
    p1 = 1 - p0

    # print(p0)

    out = torch.stack([p0, p1])
    # print(f"stacked: \n {out}.\nshape {out.shape}")
    out = out[actions.flatten().long(), range(len(actions))]
    # print(f"selected: \n {out}.\nshape {out.shape}")
    out = -torch.log(out) * rewards

    # print(f'rewards: {rewards}')
    # print(f'out: {out}')
    # print(f'Calling backward()...')
    # print(out)
    # print(out.min())
    # print(out.max())
    out = out.mean()

    optim.zero_grad()
    # net.zero_grad()
    out.backward()

    # with torch.no_grad():
    #     for p in net.parameters():
    #         print(p.grad * 0.01)
    #         p -= p.grad * 0.01
    optim.step()

    print(f'Epoch: {epoch}')
    # print(list(net.parameters()))
    epoch += 1


class MyTCPHandler(socketserver.BaseRequestHandler):
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

    def handle(self):
        number_of_episodes = self.receive_int()

        states = []
        actions = []
        rewards = []
        total = []

        for ep in range(number_of_episodes):
            states += self.receive_vector_array()
            actions += self.receive_vector_array()

            r = self.receive_vector()
            total.append(sum(r))

            to_discounted(r)

            rewards += r

        states = torch.tensor(states)
        actions = torch.tensor(actions)
        rewards = torch.tensor(rewards)

        print(f"rewards: mean total reward: {np.mean(total)}")
        # print(rewards)
        # print(rewards.mean())
        # print(rewards.mean())
        std = rewards.std()

        rewards = (rewards - rewards.mean()) / std

        if std > 0:
            train_net(states, actions, rewards)

        self.request.sendall(net_to_bytes(net))


if __name__ == "__main__":
    HOST, PORT = socket.gethostname(), 11000
    device = torch.device('cuda' if torch.cuda.is_available() else 'cpu')

    net = Linear(1, 1)
    # net.to(device)
    optim = SGD(net.parameters(), lr=0.01, momentum=0.9)

    server = socketserver.TCPServer((HOST, PORT), MyTCPHandler)

    print("Server started")

    server.serve_forever()
