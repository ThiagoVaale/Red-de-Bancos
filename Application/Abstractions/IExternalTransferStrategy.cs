using Application.DTOs;
using Domine.Entities;
using Domine.Enums;
using GoodBank.Application.Models;

namespace GoodBank.Application.Interfaces;
public interface IExternalTransferStrategy
{

    BankCode SupportedBank { get; }
    public Task<ExternalTransferResult> SendAsync(ExternalTransferRequestDto request, CancellationToken ct);

    public class WorseBankStrategy;
        

}