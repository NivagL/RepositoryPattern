﻿//using Model.Serialiser;

namespace Model.Abstraction;

public class GuidKeyModel<TValue> : DefaultKeyModel<Guid, TValue>
{
    public GuidKeyModel()
    {
        KeysEqual = (x, y) => Guid.Equals(x, y);
        NewKey = () => Guid.NewGuid();
        IsKeyTuple = false;
        //KeySerialiser = new GuidSerialiser();
    }
}
