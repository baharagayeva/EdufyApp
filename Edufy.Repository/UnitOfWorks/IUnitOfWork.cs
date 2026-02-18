namespace Edufy.Repository.UnitOfWorks;

public interface IUnitOfWork
{
    void Commit();
    Task<int> CommitAsync();
    //IReceiverProfileRepository ReceiverProfileRepository { get; }
}