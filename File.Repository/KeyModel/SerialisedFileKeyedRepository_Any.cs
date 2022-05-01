using Repository.Abstraction;

namespace SerialisedFile.Repository
{
    public partial class SerialisedFileKeyedRepository<TKey, TValue>
        : IKeyedRepositoryAny<TKey, TValue>
    {
    }
}
