using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;

namespace NZWalks.API.Controllers
{
    //https://localhost:port_number/api/regions {when running the application with this url it will point to this controller}
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //This method will return all the regions
        //GET ALL REGIONS
        //GET : https:localhost:port_number/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            var regions = dbContext.Regions.ToList();
            return Ok(regions);
        }

        //GET a single region(Get Region by Id)
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetRegionById([FromRoute]Guid id)
        {
            //First way to get region by id.
            //Find method only takes the primary key
            //var region = dbContext.Regions.Find(id);

            //2nd way to get Region by id 
            var region = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (region == null)
            {
                return NotFound();
            }
            return Ok(region);
        }
    }
}
