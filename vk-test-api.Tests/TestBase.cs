using Microsoft.Extensions.Caching.Memory;
using vk_test_api.Models;
using vk_test_api.Utils;

namespace vk_test_api.Tests;

public abstract class TestBase : IDisposable
{
    protected readonly ApiContext context;
    protected readonly IMemoryCache cache;

    public TestBase()
    {
        context = ApiContextFactory.Create();
        cache = new MemoryCache(new MemoryCacheOptions { ExpirationScanFrequency = TimeSpan.FromSeconds(5) });
    }

    public void Dispose()
    {
        ApiContextFactory.Destroy(context);
        cache.Dispose();
    }

    protected static void FillDataBase(ApiContext context)
    {
        var sampleArray = new[]
        {
            ("John Smith", "CRxXNUblDKoSnYfbdse"),
            ("John Doe", "LtRRekyOclY"),
            ("Jane Bauer", "nHjTxRRzFsn"),
            ("Jane Schneider", "RrNsvriFHiDuQqZxtn"),
            ("Jane Lefebvre", "wEPSrFVQlw"),
            ("Jane Dubois", "knPFQltYpMGUR"),
            ("Jane Doe", "hIRpVexzBkUBujqSyCMM"),
            ("Jane Smith", "dCoXRbbXSzcvtelZjhQW"),
            ("John Semerenko", "yPkJkKxnEWU"),
            ("John Dorosh", "vuBbqwizmjVPnWMCdE"),
            ("John Suzuki", "MxxWixxnmtqZcNYvf"),
            ("John Takahashi", "jDcLzIqwVKHjxt"),
            ("John Bauer", "jyQRWZNUKjhEhOUe"),
            ("John Schneider", "KEmanONCCLKWlDokYWTz"),
            ("John Lefebvre", "vAxQZTbuCLqL")
        };
        var sample = new List<User>();
        var i = 1;
        foreach (var item in sampleArray)
        {
            sample.Add(new User
            {
                Id = i,
                Login = item.Item1,
                Password = item.Item2,
                CreatedDate = DateTime.UtcNow + TimeSpan.FromDays(new Random().Next(-2000, 2000)),
                UserGroupId = (int)UserGroupCodes.User,
                UserStateId = (int)UserStateCodes.Active
            });
            i++;
        }
        context.AddRange(sample);
        context.SaveChanges();
    }
}
