using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class F {
    public static float Sigmoid(this float x) {
        return 1 / (1 + Mathf.Exp(-x));
    }
}