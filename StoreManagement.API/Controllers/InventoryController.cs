using Microsoft.AspNetCore.Mvc;
using StoreManagement.Application.InventoryManagement;
using StoreManagement.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace StoreManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class InventoryController : ControllerBase
    {
        private readonly GetAllInventory _getAllInventory;
        private readonly GetInventoryById _getInventoryById;
        private readonly AddInventory _addInventory;
        private readonly UpdateInventory _updateInventory;
        private readonly DeleteInventory _deleteInventory;

        public InventoryController(GetAllInventory getAllInventory,GetInventoryById getInventoryById,AddInventory addInventory,UpdateInventory updateInventory,DeleteInventory deleteInventory)
        {
            _getAllInventory = getAllInventory;
            _getInventoryById = getInventoryById;
            _addInventory = addInventory;
            _updateInventory = updateInventory;
            _deleteInventory = deleteInventory;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var inventories = await _getAllInventory.ExecuteAsync();// هون بكل بساطه ستخدمنا اليوز كيس مع الداتا يلي تحولت منها في كلاس المنطق

            return Ok(inventories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var inventory = await _getInventoryById.ExecuteAsync(id); // نفس الشي اليوز كيس المنطق مع فحص بسيط

            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(inventory);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateInventoryDto dto)
        {
            var addedInventory = await _addInventory.ExecuteAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = addedInventory.Id }, addedInventory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateInventoryDto dto)
        {
            var result = await _updateInventory.ExecuteAsync(id, dto);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _deleteInventory.ExecuteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}