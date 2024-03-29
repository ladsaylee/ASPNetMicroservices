﻿using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controller
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController:ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly IProductRepository _repository;

        public CatalogController(IProductRepository repository, ILogger<CatalogController>logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetProducts();
            return Ok(products);
        }


        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetProductbyId(string id)
        {
            var product = await _repository.GetProduct(id);
            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }
            return Ok(product);
        }

        [Route("[action]/{category}", Name ="GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductbyCategory(string category)
        {
            var product = await _repository.GetProductByCategory(category);
            return Ok(product);
        }


        [HttpPost]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateProduct([FromBody] Product p)
        {
          await _repository.CreateProduct(p);
           return CreatedAtRoute("GetProduct", new { id = p.Id }, p);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _repository.UpdateProduct(product));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            return Ok(await _repository.DeleteProduct(id));
        }

    }
}
