using CachingLibrary.Common.Abstractions.Dtos.GetProducts;
using CachingLibrary.Common.Abstractions.Interfaces.Services.Caching;
using CachingLibrary.Common.Utilities.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CacheServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private string[] Summaries = new[]
      {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private string[] Products = new[]
{
            "pen", "book", "notebook", "eraser", "scarf", "chair", "printer", "pencil"
        };


        public ICacheService _cacheService { get; set; }
        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        //public IInMemoryCacheService _inMemoryCacheService { get; set; }

        //public CacheController(IInMemoryCacheService inMemorycacheService)
        //{
        //    _inMemoryCacheService = inMemorycacheService;
        //}

        //[HttpGet("GetKeys")]
        //public List<string> GetKeys()
        //{
        //    return _inMemoryCacheService.GetKeys();
        //}

        [HttpGet("GetSummaries")]
        [Cache]
        public List<string> GetSummaries([FromQuery]int count)
        {
            Console.WriteLine("Some get service logic here......");
            return [.. Summaries.Take(count)];
        }

        [HttpGet("GetProducts")]
        [Cache]
        public List<string> GetProducts([FromQuery]GetProductsReqDto request)
        {
            Console.WriteLine("Some get service logic here......");
            return [.. Products.Where(p=>p.StartsWith(request.name)).Take(request.count)];
        }


        //[HttpGet("GetProducts/{name}/{count}")]
        //[Cache]
        //public List<string> GetProducts(string surname, int count)
        //{
        //    Console.WriteLine("Some get service logic here......");
        //    return [.. Products.Where(p=>p.StartsWith(surname)).Take(count)];
        //}


        //[HttpGet("GetProducts1")]
        //[Cache]
        //public List<string> GetProducts([FromQuery]GetProductsReqDto request,string synam, int age)
        //{
        //    Console.WriteLine("Some get service logic here......");
        //    return [.. Products.Where(p=>p.StartsWith(synam)).Take(age)];
        //}


        [HttpDelete("DeleteSummary")]
        [RemoveStoredCache(RemovePatternKey ="GetSumma")]
        public void DeleteSummary([FromQuery]string name)
        {
            Console.WriteLine("Some Delete service logic here......");
            Summaries = [.. Summaries.Where(r => r != name)];
        }
    }
}
