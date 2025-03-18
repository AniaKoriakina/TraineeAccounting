using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Application.Dtos;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;
using TraineeAccounting.Domain.Models;

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
    public async Task<ActionResult<IEnumerable<TraineeDto>>> GetAllTrainees([FromQuery] SearchAndSortRequest request, CancellationToken cancellationToken)
    {
        var trainees = await _traineeRepository.GetPaginatedAsync(request, cancellationToken);
        var dtos = trainees.Items.Select(x => new TraineeDto
        {
            TraineeId = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Gender = x.Gender,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            DateOfBirth = x.DateOfBirth,
            ProjectId = x.Project.ProjectId,
            ProjectName = x.Project?.Name,
            TraineeshipId = x.Traineeship.TraineeshipId,
            TraineeshipName = x.Traineeship?.Name,
        }).ToList();
        return Ok(new PagedResult<TraineeDto>
        {
            Items = dtos,
            TotalCount = trainees.TotalCount,
            PageSize = request.PageSize,
            PageIndex = request.PageIndex,
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TraineeDto>> GetById(int id)
    {
        var trainees = await _traineeRepository.GetByIdAsync(id);
        if (trainees == null) return NotFound();
        var dtos = new TraineeDto
        {
            TraineeId = trainees.Id,
            FirstName = trainees.FirstName,
            LastName = trainees.LastName,
            Gender = trainees.Gender,
            Email = trainees.Email,
            PhoneNumber = trainees.PhoneNumber,
            DateOfBirth = trainees.DateOfBirth,
            ProjectId = trainees.Project.ProjectId,
            ProjectName = trainees.Project?.Name,
            TraineeshipId = trainees.Traineeship.TraineeshipId,
            TraineeshipName = trainees.Traineeship?.Name,
        };
        return Ok(dtos);
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