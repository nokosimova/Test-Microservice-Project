using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(IPlatformRepository repository, 
                                    IMapper mapper,
                                    ICommandDataClient commandDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms ....");

            var platformItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name = "GetPlatform") ]
        public ActionResult<PlatformReadDto> GetPlatform(int id)
        {
            Console.WriteLine("--> Getting Platform by id ....");

            var platformItem = _repository.GetPlatformById(id);

            if (platformItem == null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platform)
        {
            var newEntity = _mapper.Map<Platform>(platform);
            _repository.CreatePlatform(newEntity);
            _repository.SaveChange();

            var resultDto = _mapper.Map<PlatformReadDto>(newEntity);

            //test synch messaging with command service:
            try
            {
                await _commandDataClient.SendPlatformCommand(resultDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send synchroniosly: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatform), new { id = resultDto.Id }, resultDto);

        }
    }
}