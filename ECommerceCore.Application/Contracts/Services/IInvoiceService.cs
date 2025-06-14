﻿using ECommerceCore.Application.Common.Results;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Application.Contracts.ViewModels.Invoices;

namespace ECommerceCore.Application.Contracts.Services
{
    public interface IInvoiceService
    {
        Task<PaginatedResult<InvoiceDto>> GetInvoicesPaginatedAsync(InvoiceQueryParameters queryParams);
    }
}
