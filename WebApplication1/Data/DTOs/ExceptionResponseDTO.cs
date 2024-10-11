using System.Net;

namespace WebApplication1.Data.DTOs;

public record ExceptionResponseDTO(HttpStatusCode StatusCode, string Description);