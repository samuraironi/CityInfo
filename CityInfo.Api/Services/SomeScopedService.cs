using CityInfo.Api.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Api.Services
{
	public class SomeScopedService : ISomeScopedService
	{
		private CityInfoCintext _cityInfoContext;
		private ICityCRUD _cityCrud;

		public SomeScopedService(CityInfoCintext cityInfoContext, ICityCRUD cityCrud)
        {
			_cityInfoContext = cityInfoContext ?? throw new ArgumentNullException(nameof(cityInfoContext));
			_cityCrud = cityCrud ?? throw new ArgumentNullException(nameof(cityCrud));
		}

		public async Task DoSomethingAsync(int id)
		{
			await Task.Delay(100);

			var city = await _cityInfoContext.Cities.FirstOrDefaultAsync(x => x.Id == id);

			var city2 = await _cityInfoContext.Cities.FirstOrDefaultAsync(x => x.Id == id);

			//var city = _cityCrud.GetCity(id);
			//var city2 = _cityCrud.GetCity(id);
		}
	}
}
