using Ecommerce.Data.Interfaces;
using Ecommerce.Data.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ecommerce.Api.DTOs.EcommerceStore;
using Microsoft.AspNetCore.Authorization;

/*---------------------------------------------------------------------------------------------------*/
namespace Ecommerce.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]

/*---------------------------------------------------------------------------------------------------*/
    public class EcommerceStoreController : ControllerBase
    {
        private readonly IEcommerceStoreRepository _repository;
        private readonly IMapper _mapper;

        public EcommerceStoreController(IEcommerceStoreRepository repository, IMapper mapper)
        {
            _repository = repository; // repository handles DB access
            _mapper = mapper;         // mapper handles DTO to Entity
        }

        /* ---------------------Get: api/Ecommerce---------------------------------------*/
        
        [HttpGet]
        public async Task<ActionResult<List<ReadEcommerceStoreDto>>> GetAllItem(int pageNumber = 1, int pageSize = 10, string? sortBy = "Product")
        {
            // 1. Getting data from repository (entities from DB)
            var items = await _repository.GetAllGoodsAsync();

            // 2. Mapping entities → DTOs
            var dtoList = _mapper.Map<List<ReadEcommerceStoreDto>>(items);

            // 3. Return DTOs to API response
            return Ok(dtoList);
        }

        /*------------------GetById: api/Ecommerce/{id}---------------------------------------*/
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadEcommerceStoreDto>> GetItemById(int id)
        {
            // 1. Get single entity from repository
            var item = await _repository.GetGoodsByIdAsync(id);
            //Checking if found or not
            if (item == null)
            {
                return NotFound($"Goods with Id:{id} was not found");
            }
            // 2. Mapping entities → DTOs
            var dto = _mapper.Map<List<ReadEcommerceStoreDto>>(item);

            return Ok(dto);
        }


        /*------------------Post: api/Ecommerce---------------------------------------*/
        [HttpPost]
        public async Task<ActionResult<ReadEcommerceStoreDto>> CreateGoods(CreateEcommerceStoreDto dto)
        {
            // 1. Mapping incoming DTO → Entity
            var ecommerceStore = _mapper.Map<EcommerceStore>(dto);

            // 2. Save entity using repository
            await _repository.AddGoodsAsync(ecommerceStore);

            // 3. Map back Entity → Read DTO (to return)
            var readDto = _mapper.Map<ReadEcommerceStoreDto>(ecommerceStore);

            // 4. Return response (201 Created) with location header
            return CreatedAtAction(nameof(GetItemById), new { id = readDto.Id }, readDto);
        }

        /*------------------Put: api/Ecommerce/{id}---------------------------------------*/
        [HttpPut("{id}")]
        public async Task<ActionResult<ReadEcommerceStoreDto>> UpdateGoods(int id, UpdateEcommerceStoreDto dto)
        {
            // 1. Get the existing entity from repository
            var existingGoods = await _repository.GetGoodsByIdAsync(id);
            if (existingGoods == null)
            {
                return NotFound($"Goods with Id:{id} was not found");
            }

            // 2. Map DTO → existing entity (updates properties)
            _mapper.Map(dto, existingGoods);

            // 3. Save changes
            await _repository.UpdateGoodsAsync(id, existingGoods);

            // 4. Map back Entity → DTO
            var readDto = _mapper.Map<ReadEcommerceStoreDto>(existingGoods);

            // 5. Return updated object
            return Ok(readDto);
        }

        /*------------------Delete: api/Ecommerce/{id}---------------------------------------*/
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGoods(int id)
        {
            var existingGoods = await _repository.DeleteGoodsByIdAsync(id);
            if (existingGoods == null)
            {
                return NotFound($"Goods with Id:{id} was not found");
            }

            return Ok(existingGoods);
        }



    }
}
