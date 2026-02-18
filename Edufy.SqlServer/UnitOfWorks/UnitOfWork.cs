using Edufy.Repository.UnitOfWorks;

namespace Edufy.SqlServer.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly string _connectionString;
   // protected readonly TransferDbContext Context;

    //private ReceiverProfileRepository? _receiverProfileRepository;

    // public UnitOfWork(TransferConfig config, TransferDbContext context)
    // {
    //     _connectionString = (config.GetConnectionString())!;
    //     Context = context;
    // }

    // public IReceiverProfileRepository ReceiverProfileRepository =>
    //     _receiverProfileRepository ??= new ReceiverProfileRepository(_connectionString, Context);
    

    public void Commit()
    {
        //Context.SaveChanges();
    }

    public async Task<int> CommitAsync()
    {
        //return await Context.SaveChangesAsync();
        return 1;
    }
}