﻿using System;

namespace TSP
{
    public static class RandomGenerator
    {
        private static Random _global = new Random();
        [ThreadStatic]
        private static Random _local;

        public static Random GetInstance()
        {
            Random inst = _local;
            if (inst == null)
            {
                int seed;
                lock (_global) seed = _global.Next();
                _local = inst = new Random(seed);
            }
            return inst;
        }
    }
}
