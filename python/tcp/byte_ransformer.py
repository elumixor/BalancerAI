import struct

import torch


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
