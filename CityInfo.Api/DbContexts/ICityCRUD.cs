using CityInfo.Api.Entities;

namespace CityInfo.Api.DbContexts
{
	public interface ICityCRUD
	{
		Task<City?> GetCity(int id);
	}
}