using Ernaehrbar.Parts.Commands;
using Ernaehrbar.Parts.Ports;
using MediatR;

namespace Ernaehrbar.Parts.Handlers;

/// <summary>
/// Handler für CreateNotificationCommand.
/// </summary>
public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, CreateNotificationResult>
{
    private readonly INotificationRepository _repository;
    private readonly IUserRepository _userRepository;

    public CreateNotificationCommandHandler(
        INotificationRepository repository,
        IUserRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<CreateNotificationResult> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {request.UserId} not found");
        }

        var notificationDto = new NotificationDto(
            Id: null,
            UserId: request.UserId,
            Type: request.Type,
            Message: request.Message,
            IsRead: false,
            ReadAt: null,
            ActionLink: request.ActionLink
        );

        var notificationId = await _repository.AddAsync(notificationDto, cancellationToken);

        return new CreateNotificationResult(notificationId);
    }
}
