﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

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
        public async Task<IActionResult> GetAll()
        {
            //Get Data from Database-Domain Models
            var regionsDomain = await dbContext.Regions.ToListAsync();

            //Map Domain Models to DTOs
            var regionDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageUrl = regionDomain.RegionImageUrl,

                });
            }

            //Return Dtos back to the client ,We never return domain models
            return Ok(regionDto);
        }

        //GET a single region(Get Region by Id)
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult >GetRegionById([FromRoute] Guid id)
        {
            //Get Region Domain Model From database
            var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };

            //Return DTO back to client
            return Ok(regionDto);
        }
        //POST To create New region
        //POST : https:localhost:port_number/api/regions
        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map or Convert DTO to Domain Model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl,
            };
            //Id will be created by Entity Framework

            //Use Domain  Model to Create Region
           await dbContext.Regions.AddAsync(regionDomainModel);
            await dbContext.SaveChangesAsync();

            //Map DomainModel to Dto

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            //POST method don't send a ok response instead it send a CreatedAtAction()
            return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
        }

        //PUT To Update Region
        //PUT :https:localhost:port_number/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, UpdateRequestRegionDto updateRequestRegionDto)
        {
            //check if the region exist
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Map Dto to Domain Model
            regionDomainModel.Code = updateRequestRegionDto.Code;
            regionDomainModel.Name = updateRequestRegionDto.Name;
            regionDomainModel.RegionImageUrl = updateRequestRegionDto.RegionImageUrl;

            //After Update save the changes in dbContext
           await dbContext.SaveChangesAsync();

            //Map Domain Model to DTO

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };
            return Ok(regionDto);
        }

        //Delete a Region
        //DELETE :https:localhost:port_number/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            {
                //check if the region  exists
                var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                //Remove method don't have async method
                 dbContext.Regions.Remove(regionDomainModel);
                await dbContext.SaveChangesAsync();

                //return Ok();

                //optional you can also return the deleted data

                //Map regionDomain model to DTO

                var regionDto = new RegionDto
                {
                    Id = regionDomainModel.Id,
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl,

                };
                return Ok(regionDto);
            }
        }

        }
}

