using MediatR;

public record TestRequest(string UserName) : IRequest;