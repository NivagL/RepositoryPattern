using Repository.Model;

namespace Repository.Test.Model;

public class SimpleKeyTestModelMeta : GuidKeyModel<SimpleKeyTestModel>
{
    public SimpleKeyTestModelMeta()
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
