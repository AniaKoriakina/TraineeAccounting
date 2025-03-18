using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Application.Dtos;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;
using TraineeAccounting.Domain.Models;

namespace TraineeAccounting.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TraineeshipController : ControllerBase
{
    private readonly ITraineeshipRepository _traineeshipRepository;
    public readonly IValidator<UpdateTraineeshipTraineesCommand> _traineeshipUpdateValidator;
    private readonly IValidator<CreateTraineeshipCommand> _traineeshipCreateValidator;
    private readonly IMediator _mediator;

    public TraineeshipController(
        ITraineeshipRepository traineeshipRepository, 
        IMediator mediator, 
        IValidator<UpdateTraineeshipTraineesCommand> traineeshipUpdateValidator, 
        IValidator<CreateTraineeshipCommand> traineeshipCreateValidator)
    {
        _traineeshipRepository = traineeshipRepository;
        _mediator = mediator;
        _traineeshipUpdateValidator = traineeshipUpdateValidator;
        _traineeshipCreateValidator = traineeshipCreateValidator;
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateTraineeshipCommand command)
    {
        var validationResult = _traineeshipCreateValidator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var traineeshipId = await _mediator.Send(command);
        return traineeshipId;
    }

    [HttpGet("{traineeshipId}/trainees")]
    public async Task<ActionResult<List<TraineeDto>>> GetTraineesByTraineeship(int traineeshipId)
    {
        var result = await _mediator.Send(new GetTraineesByTraineeshipCommand { TraineeshipId = traineeshipId });
        if (result == null || !result.Any()) return NotFound();
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<TraineeshipDto>> GetTraineeships([FromQuery] SearchAndSortRequest request, CancellationToken cancellationToken)
    {
        var traineeships = await _traineeshipRepository.GetPaginatedAsync(request, cancellationToken);
        var dtos = traineeships.Items.Select(x => new TraineeshipDto
        {
            Id = x.TraineeshipId, 
            Name = x.Name,
            TraineeCount = x.Trainees.Count
        }).ToList();
        return Ok(new PagedResult<TraineeshipDto>
        {
            Items = dtos,
            TotalCount = traineeships.TotalCount,
            PageSize = request.PageSize,
            PageIndex = request.PageIndex
        });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<TraineeshipDto>> DeleteTraineeship(int id)
    {
        try
        {
            var result = await _mediator.Send(new DeleteTraineeshipCommand { TraineeshipId = id });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Conflict(ex.Message);
        };
    }

    [HttpPut("{id}/trainees")]
    public async Task<IActionResult> UpdateTrainees(int id, [FromBody] List<int> traineeIds)
    {
        if (traineeIds == null || !traineeIds.Any())
        {
            return BadRequest("Список стажеров пуст");
        }

        var command = new UpdateTraineeshipTraineesCommand
        {
            TraineeshipId = id,
            TraineeIds = traineeIds
        };
        
        var validationResult = _traineeshipUpdateValidator.Validate(command);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(error => new {error.PropertyName, error.ErrorMessage}).ToList());
        }
        
        var result = await _mediator.Send(command);

        return Ok(result); 
    }
}