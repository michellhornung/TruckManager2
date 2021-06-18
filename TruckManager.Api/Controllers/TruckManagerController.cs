using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TruckManager.Application.Interfaces;
using TruckManager.Domain.ViewModels;

namespace TruckManager.Api.Controllers
{
    [ApiController]
    [Route("api/truck")]
    public class TruckManagerController : ControllerBase
    {
        private readonly ILogger<TruckManagerController> _logger;
        private readonly ITruckManagerApplication _application;

        public TruckManagerController(ILogger<TruckManagerController> logger, ITruckManagerApplication application)
        {
            _logger = logger;
            _application = application;
        }

        [HttpGet]
        [HttpGet("search/{search}")]
        public async Task<IActionResult> GetTruck(string search)
        {
            try
            {
                var result = await _application.List(search);
                return base.Ok(new ResulViewModel<List<TruckViewModel>>(result));
            }
            catch (System.Exception ex)
            {
                return base.StatusCode(500, new ResulViewModel<List<TruckViewModel>>(ex));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTruck(int id)
        {
            try
            {
                var result = await _application.Get(id);
                return base.Ok(new ResulViewModel<TruckViewModel>(result));
            }
            catch (System.Exception ex)
            {
                return base.StatusCode(500, new ResulViewModel<TruckViewModel>(ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTruck([FromBody] TruckViewModel item)
        {
            try
            {
                var result = await _application.Add(item);
                return base.Ok(new ResulViewModel<TruckViewModel>(result));
            }
            catch (System.Exception ex)
            {
                return base.StatusCode(500, new ResulViewModel<TruckViewModel>(ex));
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditTruck([FromBody] TruckViewModel item)
        {
            try
            {
                var result = await _application.Edit(item);
                return base.Ok(new ResulViewModel<TruckViewModel>(result));
            }
            catch (System.Exception ex)
            {
                return base.StatusCode(500, new ResulViewModel<TruckViewModel>(ex));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTruck(int id)
        {
            try
            {
                await _application.Delete(id);
                return base.Ok(new ResulViewModel<TruckViewModel>());
            }
            catch (System.Exception ex)
            {
                return base.StatusCode(500, new ResulViewModel<TruckViewModel>(ex));
            }
        }
    }
}
