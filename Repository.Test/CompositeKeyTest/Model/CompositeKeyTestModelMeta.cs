using Repository.Model;
using System;

namespace Repository.Test;

public class CompositeKeyTestModelMeta 
    : DefaultKeyModel<Tuple<Guid, DateTime>, CompositeKeyTestModel>
{
    public CompositeKeyTestModelMeta()
    {
        GetKey = x => Tuple.Create(x.Id, x.Date);
        SetKey = (x, y) => { x.Id = y.Item1; x.Date = y.Item2; };
        
        Assign = (x, y) =>
        {
            x.Id = y.Id;
            x.Date = y.Date;
            x.Description = y.Description;
            x.Processed = y.Processed;
        };

        Differ = (x, y) =>
        {   return x.Date != y.Date
                || x.Description != y.Description
                || x.Processed != y.Processed;
        };
    }
}
