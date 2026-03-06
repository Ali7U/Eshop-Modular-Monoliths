namespace Ordering.Orders.ValueObjects;

public record Address
{
    public string FirstName { get; } = default!;
    public string LastName { get; } = default!;
    public string? EmailAddress { get; } = default!;
    public string AddressLine { get; } = default!;
    public string Country { get; } = default!;
    public string State { get; } = default!;
    public string ZipCode { get; } = default!;
    
    protected Address() { }

    private Address(string firstName, string lastName, string addressLine, string country, string state, string zipCode,
        string? emailAddress = null)
    {
        FirstName = firstName;
        LastName = lastName;
        AddressLine = addressLine;
        Country = country;
        State = state;
        ZipCode = zipCode;
        EmailAddress = emailAddress;
    }

    public static Address Of(string firstName, string lastName, string addressLine, string country, string state,
        string zipCode, string emailAddress)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);

        return new Address(firstName, lastName, addressLine, country, state, zipCode, emailAddress);
    }
}