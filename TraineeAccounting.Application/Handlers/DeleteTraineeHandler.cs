using MediatR;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Handlers;

public class DeleteTraineeHandler : IRequestHandler<DeleteTraineeCommand, bool>
{
    private readonly ITraineeRepository _traineeRepository;

    public DeleteTraineeHandler(ITraineeRepository traineeRepository)
    {
        _traineeRepository = traineeRepository;
    }

    public async Task<bool> Handle(DeleteTraineeCommand request, CancellationToken cancellationToken)
    {
        var trainee = await _traineeRepository.GetByIdAsync(request.TraineeId);
        if (trainee == null)
        {
            return false;
        }
        await _traineeRepository.DeleteAsync(trainee);
        return true;
    }
}