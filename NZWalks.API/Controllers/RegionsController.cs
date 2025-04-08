﻿using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAll()
        {
            //Get Data from Database-Domain Models
            var regionsDomain = dbContext.Regions.ToList();

            //Map Domain Models to DTOs
            var regionDto= new List<RegionDto>();
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
        public IActionResult GetRegionById([FromRoute]Guid id)
        {
           //Get Region Domain Model From database
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

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
        public IActionResult CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
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
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            //Map DomainModel to Dto

            var regionDto = new RegionDto
            {
                Id=regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            //POST method don't send a ok response instead it send a CreatedAtAction()
            return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
        }
    }
}
