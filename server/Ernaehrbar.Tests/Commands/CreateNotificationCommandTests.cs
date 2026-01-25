using Ernaehrbar.Parts.Commands;
using Ernaehrbar.Parts.Domain;
using Ernaehrbar.Parts.Handlers;
using Ernaehrbar.Parts.Ports;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Ernaehrbar.Tests.Commands;

/// <summary>
/// Tests für CreateNotificationCommand (TDD).
/// </summary>
public class CreateNotificationCommandTests
{
    [Fact]
    public async Task Handle_ShouldCreateNotification_WhenValidCommand()
    {
        var repository = Substitute.For<INotificationRepository>();
        var userRepository = Substitute.For<IUserRepository>();

        var userId = 1;
        var notificationId = 100;

        userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(new UserDto(userId, "test-user", "test@example.com"));

        repository.AddAsync(Arg.Any<NotificationDto>(), Arg.Any<CancellationToken>())
            .Returns(notificationId);

        var handler = new CreateNotificationCommandHandler(repository, userRepository);
        var command = new CreateNotificationCommand(
            UserId: userId,
            Type: NotificationType.UploadComplete,
            Message: "Upload erfolgreich verarbeitet",
            ActionLink: "/recipe-drafts/123"
        );

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(notificationId);

        await repository.Received(1).AddAsync(
            Arg.Is<NotificationDto>(n =>
                n.UserId == userId &&
                n.Type == NotificationType.UploadComplete &&
                n.Message == "Upload erfolgreich verarbeitet" &&
                n.ActionLink == "/recipe-drafts/123" &&
                n.IsRead == false),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldCreateNotification_WhenNoActionLink()
    {
        var repository = Substitute.For<INotificationRepository>();
        var userRepository = Substitute.For<IUserRepository>();

        var userId = 1;
        var notificationId = 100;

        userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(new UserDto(userId, "test-user", "test@example.com"));

        repository.AddAsync(Arg.Any<NotificationDto>(), Arg.Any<CancellationToken>())
            .Returns(notificationId);

        var handler = new CreateNotificationCommandHandler(repository, userRepository);
        var command = new CreateNotificationCommand(
            UserId: userId,
            Type: NotificationType.GenerationComplete,
            Message: "Rezept-Generierung abgeschlossen"
        );

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(notificationId);

        await repository.Received(1).AddAsync(
            Arg.Is<NotificationDto>(n =>
                n.ActionLink == null),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserNotFound()
    {
        var repository = Substitute.For<INotificationRepository>();
        var userRepository = Substitute.For<IUserRepository>();

        userRepository.GetByIdAsync(999, Arg.Any<CancellationToken>())
            .Returns((UserDto?)null);

        var handler = new CreateNotificationCommandHandler(repository, userRepository);
        var command = new CreateNotificationCommand(
            UserId: 999,
            Type: NotificationType.UploadComplete,
            Message: "Test"
        );

        await Should.ThrowAsync<InvalidOperationException>(
            () => handler.Handle(command, CancellationToken.None));

        await repository.DidNotReceive().AddAsync(Arg.Any<NotificationDto>(), Arg.Any<CancellationToken>());
    }
}
