using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Application.Dtos;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;
using TraineeAccounting.Domain.Models;

namespace TraineeAccounting.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;
    private readonly IValidator<UpdateProjectTraineesCommand> _projectUpdateValidator;
    private readonly IValidator<CreateProjectCommand> _createProjectValidator;
    private readonly IMediator _mediator;

    public ProjectController(
        IProjectRepository projectRepository, 
        IMediator mediator, 
        IValidator<UpdateProjectTraineesCommand> projectUpdateValidator, 
        IValidator<CreateProjectCommand> createProjectValidator)
    {
        _projectRepository = projectRepository;
        _mediator = mediator;
        _projectUpdateValidator = projectUpdateValidator;
        _createProjectValidator = createProjectValidator;
    }

    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
    // {
    //     var projects = await _projectRepository.GetAllAsync();
    //     return Ok(projects.Select(x => new ProjectDto { Id = x.ProjectId, Name = x.Name }));
    // }
    [HttpGet("{projectId}/trainees")]
    public async Task<ActionResult<List<TraineeDto>>> GetTraineesByProject(int projectId)
    {
        var result = await _mediator.Send(new GetTraineesByProjectCommand { ProjectId = projectId });
        if (result == null || !result.Any()) return NotFound();
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<ActionResult<ProjectDto>> GetProjects([FromQuery] SearchAndSortRequest request, CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetPaginatedAsync(request, cancellationToken);
        var dtos = projects.Items.Select(x => new ProjectDto
        {
            Id = x.ProjectId, 
            Name = x.Name,
            TraineeCount = x.Trainees.Count
        }).ToList();
        return Ok(new PagedResult<ProjectDto>
        {
            Items = dtos,
            TotalCount = projects.TotalCount,
            PageSize = request.PageSize,
            PageIndex = request.PageIndex
        });
    }

    [HttpDelete("{id}")]

    public async Task<ActionResult<ProjectDto>> DeleteProject(int id)
    {
        try
        {
            var result = await _mediator.Send(new DeleteProjectCommand() { ProjectId = id });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Conflict(ex.Message);
        };
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateProjectCommand command)
    {
        var validationResult = _createProjectValidator.Validate(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var projectId = await _mediator.Send(command);
        return projectId;
    }
    
    [HttpPut("{id}/trainees")]
    public async Task<IActionResult> UpdateTrainees(int id, [FromBody] List<int> traineeIds)
    {
        if (traineeIds == null || !traineeIds.Any())
        {
            return BadRequest("Список стажеров пуст");
        }

        var command = new UpdateProjectTraineesCommand
        {
            ProjectId = id,
            TraineeIds = traineeIds
        };
        
        var validationResult = _projectUpdateValidator.Validate(command);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(error => new {error.PropertyName, error.ErrorMessage}).ToList());
        }
        
        var result = await _mediator.Send(command);

        return Ok(result); 
    }
}