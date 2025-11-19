public class User
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Cpf { get; private set; } = null!;
    public DateTime BirthDate { get; private set; }
    public string PasswordHash { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    private User() { }

    public User(string fullName, string email, string cpf, DateTime birthDate, string passwordHash)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Email = email;
        Cpf = cpf;
        BirthDate = birthDate;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string fullName, string email, string cpf, DateTime birthDate, string? passwordHash)
    {
        FullName = fullName;
        Email = email;
        Cpf = cpf;
        BirthDate = birthDate;

        if (!string.IsNullOrWhiteSpace(passwordHash))
            PasswordHash = passwordHash;
    }

}
