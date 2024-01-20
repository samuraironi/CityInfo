using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    [Route("api/cities/{cityid}/pointsofinterest")]
    [ApiController]
    public class PointOfInterestsController : ControllerBase
    {
        private ILogger<PointOfInterestsController> _logger;
        private IMailService _localMailService;
        private CitiesDataStore _citiesDataStore;

        public PointOfInterestsController(ILogger<PointOfInterestsController> logger,
            IMailService localMailService,
            CitiesDataStore citiesDataStore) 
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _localMailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));

            //another injection example
            //var logger = HttpContext.RequestServices.GetService<ILogger<PointOfInterestsController>>();
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);

            if(city == null) 
            {
                _logger.LogInformation($"City with id {cityId} wans't found when trying to access points of interest");
                return NotFound();
            }

            return Ok(city.PointOfInterests);
        }

        [HttpGet("{pointofInterestid}",  Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPoointOfInstrest(int cityId, int pointofInterestid) 
        {
            try
            {
                var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);

                if (city == null)
                {
                    return NotFound();
                }

                var pointOfInsterest = city.PointOfInterests.FirstOrDefault(x => x.Id == pointofInterestid);

                if (pointOfInsterest == null)
                {
                    return NotFound();
                }

                return Ok(pointOfInsterest);
            }
            catch (Exception ex) 
            {
                _logger.LogCritical($"Exception while getting points of interest for a city with id ${cityId}", ex);
                return StatusCode(500, "A problem happened with your request");
            }
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
        {
            //is checked automatically for input object using api annotations and returns bar request automatically
            /*if(!ModelState.IsValid)
            {
                return BadRequest();
            }*/

            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var maxPointOfInterest = _citiesDataStore.Cities.SelectMany(x => x.PointOfInterests).Max(x => x.Id);
            var finalPointOfInterestDto = new PointOfInterestDto
            {
                Id = ++maxPointOfInterest,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description,
            };

            city.PointOfInterests.Add(finalPointOfInterestDto);
            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, pointOfInterestId = finalPointOfInterestDto.Id }, finalPointOfInterestDto);
        }

        [HttpPut("{pointOfInsterestId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInsterestId, PointOfInterestForUpdateDto pointOfInterest)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointOfInterests.FirstOrDefault(x => x.Id == pointOfInsterestId);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{pointOfInsterestId}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInsterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointOfInterests.FirstOrDefault(x => x.Id == pointOfInsterestId);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch =
                new PointOfInterestForUpdateDto()
                {
                    Name = pointOfInterestFromStore.Name,
                    Description = pointOfInterestFromStore.Description,
                };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("pointOfInterestId")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointOfInterests.FirstOrDefault(x => x.Id == pointOfInterestId);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            city.PointOfInterests.Remove(pointOfInterestFromStore);
            _localMailService.Send(
                "Point of inteterest deleted.", 
                    $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Name} was deleted");
            return NoContent();
        }
    }
}
