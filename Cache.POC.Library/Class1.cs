namespace Cache.POC.Library
{
    public interface ICache
    {
        T GetOr<T>(string key, Func<T> compute);
    }

    public interface IRepo
    {
        IEnumerable<string> GetAll();
        string GetById(int id);
    }

    public class Repo : IRepo
    {
        IEnumerable<string> _items = new List<string> { "one", "two" };
        public IEnumerable<string> GetAll()
            => _items;

        public string GetById(int id)
            => _items.First();
    }

    public class CachedRepo : IRepo
    {
        private readonly Repo baseService;
        private readonly ICache cache;

        public CachedRepo(Repo baseService, ICache cache)
        {
            this.baseService = baseService;
            this.cache = cache;
        }

        public IEnumerable<string> GetAll() =>
            this.cache.GetOr("GetAll", this.baseService.GetAll);

        public string GetById(int id) =>
            this.cache.GetOr($"GetById.{id}", () => this.baseService.GetById(id));
    }

    public class Service
    {
        private readonly IRepo repo;

        public Service(IRepo repo)
        {
            this.repo = repo;
        }
    }

    public class Controller
    {
        private readonly Service service;

        public Controller(Service service)
        {
            this.service = service;
        }
    }
}