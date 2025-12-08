namespace Common.Infrastructure.Seeder.Contracts
{
    public interface ISeederRunner
    {
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
