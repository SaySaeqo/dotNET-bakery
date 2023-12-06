using System;
using Microsoft.AspNetCore.Mvc;
using dotNet_bakery.Models;
using dotNet_bakery.Repo;

namespace dotNet_bakery.Controller;

[Controller]
[Route("api/[controller]")]
public class DataController: ControllerBase{

    private readonly DataRepository _dataRepository;

    public DataController(DataRepository dataRepository){
        _dataRepository = dataRepository;
    }

    [HttpGet]
    public async Task<List<DataModel>> Get(){
        return await _dataRepository.GetAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DataModel dataModel){
        await _dataRepository.CreateAsync(dataModel);
        return CreatedAtAction(nameof(Get), new {id = dataModel.Id}, dataModel);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id){
        await _dataRepository.DeleteAsync(id);
        return NoContent();
    }

}