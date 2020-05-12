import socketserver
import socket
import struct

import matplotlib
import numpy as np
import torch
from torch.nn import Linear
from torch.optim import Adam, SGD
import matplotlib.pyplot as plt

from tcp.byte_ransformer import tensor_to_bytes
from tcp.unity_handler import TorchTCPHandler, start_unity_handler


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
total_totals = []


class RLHandler(TorchTCPHandler):
    def handle(self):
        global epoch

        number_of_episodes = self.receive_int()

        total = []

        loss = torch.tensor(0., requires_grad=True)

        for episode in range(number_of_episodes):
            states = self.receive_vector_array()
            actions = self.receive_vector_array()

            rewards = self.receive_vector()
            total.append(sum(rewards))

            states = torch.tensor(states)
            actions = torch.tensor(actions)

            q_val = model.critic(states[-1])
            q_values = [0 for _ in range(len(rewards))]
            for i in reversed(range(len(rewards))):
                q_val = q_values[i] = rewards[i] + discounting * q_val

            rewards = torch.tensor(rewards)

            # rewards = (rewards - rewards.mean()) / std
            # print(f'Starting training')

            values, (p0, p1) = model.forward(states)
            q_values = torch.tensor(q_values)

            advantage = q_values - values
            # print(values)

            out = torch.stack([p0, p1])
            out = out[actions.flatten().long(), range(len(actions))]
            actor_loss = (-torch.log(out) * advantage).mean()
            critic_loss = .5 * advantage.pow(2).mean()
            loss = loss + actor_loss + critic_loss

        optim.zero_grad()
        # actor_loss.backward()
        loss.backward()
        # for p in model.parameters():
        #     print(f'{p} {p.grad}')

        optim.step()

        print(f"Epoch: {epoch} mean total reward: {np.mean(total)}")
        total_totals.append(np.mean(total))
        # if len(total_totals) % 10 == 0:
        #     plt.plot(total_totals)
        #     plt.draw()
        #     plt.pause(0.0001)
        #     plt.clf()

        epoch += 1

        self.request.sendall(net_to_bytes(model.actor))


class A2C(torch.nn.Module):
    def __init__(self, obs_size, num_actions):
        super().__init__()

        self.num_actions = num_actions
        self.critic = Linear(obs_size, 1)
        self.actor = Linear(obs_size, 1)

    def forward(self, obs):
        action = self.actor(obs)
        value = self.critic(obs).flatten()

        p0 = action.flatten().sigmoid()
        p1 = 1 - p0

        return value, (p0, p1)


if __name__ == "__main__":
    model = A2C(1, 2)
    optim = SGD(model.parameters(), lr=0.001, momentum=0.8)
    # optim = Adam(model.parameters(), lr=0.01)
    # matplotlib.use("TkAgg")
    # plt.ion()
    start_unity_handler(RLHandler)
