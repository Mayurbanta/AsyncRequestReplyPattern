namespace Core.Contracts;

public interface IJobService
{
    void Handle(Guid jobId);
}



