using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Controllers;

[Route("api/[controller]s")]
[ApiController]
public class TraineeController :ControllerBase
{
    private readonly IMediator _mediator;
    public readonly ITraineeRepository _traineeRepository;
    public readonly IValidator<CreateTraineeCommand> _traineeValidator;

    public TraineeController(IMediator mediator, IValidator<CreateTraineeCommand> traineeValidator, ITraineeRepository traineeRepository)
    {
        _mediator = mediator;
        _traineeValidator = traineeValidator;
        _traineeRepository = traineeRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Trainee>>> GetAllTrainees()
    {
        var trainees = await _traineeRepository.GetTraineesAsync();
        return Ok(trainees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Trainee>> GetById(int id)
    {
        var trainee = await _traineeRepository.GetByIdAsync(id);
        return Ok(trainee);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateTraineeCommand command)
    {
        ValidationResult validationResult = _traineeValidator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var traineeId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = traineeId }, traineeId);
        
    }
}