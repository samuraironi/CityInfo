using CityInfo.Api.Models;

namespace CityInfo.Api
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
        //public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public CitiesDataStore()
        {
                Cities = new List<CityDto>()
                {
                    new CityDto()
                    {
                        Id = 1,
                        Name = "New Your City",
                        Description = "The one with that big park",
                       PointOfInterests = new List<PointOfInterestDto>()
                       {
                           new PointOfInterestDto
                           {
                               Id = 1,
                               Name = "Central Parl",
                                Description = "The most visited bla bla bla"
                           },
                           new PointOfInterestDto
                           {
                               Id = 2,
                               Name = "Emprie State Building",
                               Description = "Bla bla bla"
                           }
                       }
                    },
                    new CityDto()
                    {
                        Id = 2,
                        Name = "Antwerp",
                        Description = "The one with the ctahedral that was never...",
                        PointOfInterests = new List<PointOfInterestDto>()
                       {
                           new PointOfInterestDto
                           {
                               Id = 3,
                               Name = "Central Parl bla",
                                Description = "The most visited bla bla bla"
                           },
                           new PointOfInterestDto
                           {
                               Id = 4,
                               Name = "Emprie State Building bla",
                               Description = "Bla bla bla"
                           }
                       }
                    },
                    new CityDto()
                    {
                        Id = 3,
                        Name = "Paris",
                        Description = "The one with that big tower",
                        PointOfInterests = new List<PointOfInterestDto>()
                       {
                           new PointOfInterestDto
                           {
                               Id = 5,
                               Name = "Central Parl bla bla",
                                Description = "The most visited bla bla bla"
                           },
                           new PointOfInterestDto
                           {
                               Id = 6,
                               Name = "Emprie State Building bla bla",
                               Description = "Bla bla bla"
                           }
                       }
                    },
                };
        }
    }
}
