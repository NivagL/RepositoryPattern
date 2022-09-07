using Model.Abstraction;

namespace Test.Model;

public class SimpleGuidModelMeta 
    : GuidKeyModel<SimpleGuidModel>
{
    public SimpleGuidModelMeta()
    {
        GetKey = x => x.Id;
        SetKey = (x, y) => x.Id = y;
        
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
