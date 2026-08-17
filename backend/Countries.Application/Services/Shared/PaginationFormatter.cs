using Countries.Application.DTOs.Responses.Countries;

namespace Countries.Application.Services.Shared;

public static class PaginationFormatter
{
    public static PaginationResponse Format(RestCountriesPaginationMetaResponse? meta)
    {
        if (meta == null)
        {
            return new PaginationResponse(
                CurrentPage: 1,
                TotalPages: 1,
                TotalItems: 0,
                PageSize: 50,
                Count: 0
            );
        }

        int limit = meta.Limit > 0 ? meta.Limit : 50;
        int currentPage = (meta.Offset / limit) + 1;
        int totalPages = meta.Total > 0 ? (int)Math.Ceiling((double)meta.Total / limit) : 1;

        return new PaginationResponse(
            CurrentPage: currentPage,
            TotalPages: totalPages,
            TotalItems: meta.Total,
            PageSize: limit,
            Count: meta.Count
        );
    }
}
