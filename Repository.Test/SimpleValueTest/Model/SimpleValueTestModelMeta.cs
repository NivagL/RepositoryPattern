using Repository.Model;

namespace Repository.Test;

public class SimpleValueTestModelMeta : DefaultValueModel<SimpleValueTestModel>
{
    public SimpleValueTestModelMeta()
    {
        Assign = (x, y) =>
        {
            x.Date = y.Date;
            x.Description = y.Description;
            x.Processed = y.Processed;
        };

        Differ = (x, y) =>
        {   
            return x.Date != y.Date
                || x.Description != y.Description
                || x.Processed != y.Processed;
        };
    }
}
