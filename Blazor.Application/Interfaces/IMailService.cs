using FluentEmail.Core.Models;

namespace Blazor.Application.Interfaces;

public interface IMailService
{
    Task<SendResponse> SendAsync(string to, string subject, string body);

    Task<SendResponse> SendAsync(string to, string subject, string template, object model);
}