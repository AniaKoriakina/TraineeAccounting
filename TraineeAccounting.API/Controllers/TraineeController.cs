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
    public readonly IValidator<UpdateTraineeCommand> _updateTraineeValidator;

    public TraineeController(
        IMediator mediator, 
        IValidator<CreateTraineeCommand> traineeValidator, 
        IValidator<UpdateTraineeCommand> updateTraineeValidator, 
        ITraineeRepository traineeRepository)
    {
        _mediator = mediator;
        _traineeValidator = traineeValidator;
        _traineeRepository = traineeRepository;
        _updateTraineeValidator = updateTraineeValidator;
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

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateTraineeCommand command)
    {
        ValidationResult validationResult = _updateTraineeValidator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors
                .Select(error => new { error.PropertyName, error.ErrorMessage })
                .ToList());
        }

        var trainee = await _mediator.Send(command);
        return Ok(trainee);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var request = new DeleteTraineeCommand { TraineeId = id };
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}