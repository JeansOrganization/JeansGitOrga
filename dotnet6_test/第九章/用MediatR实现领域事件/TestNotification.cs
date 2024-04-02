using MediatR;

public record TestNotification(string UserName) : INotification;