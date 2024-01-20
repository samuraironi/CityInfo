using CityInfo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Api.DbContexts
{
	public class CityCRUD : ICityCRUD
	{
		private CityInfoCintext _cityInfoContext;

		public CityCRUD(CityInfoCintext cityInfoContext)
        {
			_cityInfoContext = cityInfoContext ?? throw new ArgumentNullException(nameof(cityInfoContext));
		}

		public async Task<City?> GetCity(int id) 
		{
			return await _cityInfoContext.Cities.FirstOrDefaultAsync(x => x.Id == id);
		}
    }
}
