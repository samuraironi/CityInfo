using CityInfo.Api.DbContexts;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Api.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private CitiesDataStore _citiesDataStore;
		private CityInfoCintext _cityInfoContext;

		public CitiesController(CitiesDataStore citiesDataStore, CityInfoCintext cityInfoContext)
        {
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
            _cityInfoContext = cityInfoContext ?? throw new ArgumentNullException( nameof(cityInfoContext));
        }
        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(_citiesDataStore.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == id);

            if(city == null) 
            {
                return NotFound();
            }
            return Ok(city);

        }

		[HttpGet("db")]
		public ActionResult<CityDto> GetCityFromDb()
		{

            int id = 1;
            //DoSomethingAsync(id).GetAwaiter().GetResult();
            //DoSomethingAsync(id);
            //DoSomethingAsync2(id).GetAwaiter().GetResult();

            DoScopedJob(id);

			var city =  _cityInfoContext.Cities.FirstOrDefaultAsync(x => x.Id == id);
          
			var city2 =  _cityInfoContext.Cities.FirstOrDefaultAsync(x => x.Id == id);

			if (city == null)
			{
				return NotFound();
			}
			return Ok(city);
		}

        private void DoScopedJob(int id)
        {
            HandleScoped(id).GetAwaiter().GetResult();

            //HttpContext.RequestServices.GetService<ISomeService>().DoSomethingAsync(id).GetAwaiter().GetResult();


			//var task = HandleScoped(id);
			//task.Wait();
			//Task.Run(async () => await task).GetAwaiter().GetResult();
			//var resul = task.WaitAndUnw;
		}

        private async Task HandleScoped(int id)
        {
			using (var scope = HttpContext.RequestServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				var handler = scope.ServiceProvider.GetRequiredService<ISomeScopedService>();
                await handler.DoSomethingAsync(id);
                //return task;
			}
		}

        private async void DoSomethingAsync(int id)
        {
            //await Task.Delay(10);

            var city = await _cityInfoContext.Cities.FirstOrDefaultAsync(x => x.Id == id);

            var city2 = await _cityInfoContext.Cities.FirstOrDefaultAsync(x => x.Id == id);
        }

		private async Task DoSomethingAsync2(int id)
		{
			await Task.Delay(100);

			var city = await _cityInfoContext.Cities.FirstOrDefaultAsync(x => x.Id == id);

			var city2 = await _cityInfoContext.Cities.FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}
