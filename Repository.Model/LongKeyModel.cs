﻿//using Model.Serialiser;

namespace Model.Abstraction;

public class LongKeyModel<TValue> : DefaultKeyModel<long, TValue>
{
    public static long KeyCounter = 0;

    public LongKeyModel()
    {
        KeysEqual = (x, y) => x == y;
        IsKeyTuple = false;
        NewKey = () => ++KeyCounter;
        //KeySerialiser = new LongSerialiser();
    }
}
