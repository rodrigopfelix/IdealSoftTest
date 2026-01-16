using IdealSoftTestServer.Application.DTOs.Phones;

namespace IdealSoftTestServer.Application.DTOs.Customers
{
    public record CustomerResponse(
        Guid Id,
        string FirstName,
        string LastName,
        List<PhoneResponse> Phones,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        DateTime? DeletedAt
    );
}
