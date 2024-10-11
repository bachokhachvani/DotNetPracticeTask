namespace WebApplication1.Data.DTOs;

public class ResponseDTO
{
    public string Message { get; set; }

    public ResponseDTO(string message)
    {
        Message = message;
    }
}