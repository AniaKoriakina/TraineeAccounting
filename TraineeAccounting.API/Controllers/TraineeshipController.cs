using Microsoft.AspNetCore.Mvc;
using TraineeAccounting.Application.Dtos;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TraineeshipController : ControllerBase
{
    private readonly ITraineeshipRepository _traineeshipRepository;

    public TraineeshipController(ITraineeshipRepository traineeshipRepository)
    {
        _traineeshipRepository = traineeshipRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TraineeshipDto>>> GetTraineeships()
    {
        var traineeships = await _traineeshipRepository.GetAllAsync();
        return Ok(traineeships.Select(x => new TraineeshipDto {Id = x.TraineeshipId, Name = x.Name}));
    }
}