
namespace IdealSoftTestServer.Application.DTOs.Phones
{
    public record PhoneResponse
    (
        Guid Id,
        string Number,
        string RegionCode,
        string Type,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        DateTime? DeletedAt
    );
}
