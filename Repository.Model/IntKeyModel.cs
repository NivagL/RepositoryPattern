﻿namespace Repository.Model;

public class IntKeyModel<TValue> : DefaultKeyModel<int, TValue>
{
    public static int KeyCounter = 0;

    public IntKeyModel()
    {
        KeysEqual = (x, y) => x == y;
        IsKeyTuple = false;
        NewKey = () => ++KeyCounter;
    }
}
