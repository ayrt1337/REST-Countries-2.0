using System.Net;

namespace Countries.Application.DTOs.Responses;

public readonly record struct ResultResponse<TData>(HttpStatusCode StatusCode, string Message, TData? Data)
{
    public static ResultResponse<TData> Create(HttpStatusCode statusCode, string message, TData? data = default) => 
        new(StatusCode: statusCode, Message: message, Data: data);

    public static ResultResponse<TData> CreateOk(string message, TData? data = default) =>
        new(StatusCode: HttpStatusCode.OK, Message: message, Data: data);

    public static ResultResponse<TData> CreateNotFound(string message, TData? data = default) =>
        new(StatusCode: HttpStatusCode.NotFound, Message: message, Data: data);

    public static ResultResponse<TData> CreateBadRequest(string message, TData? data = default) =>
        new(StatusCode: HttpStatusCode.BadRequest, Message: message, Data: data);

    public static ResultResponse<TData> CreateBadGateway(string message, TData? data = default) =>
        new(StatusCode: HttpStatusCode.BadGateway, Message: message, Data: data);

    public static ResultResponse<TData> CreateInternaServerlError(string message, TData? data = default) =>
        new(StatusCode: HttpStatusCode.InternalServerError, Message: message, Data: data);
}