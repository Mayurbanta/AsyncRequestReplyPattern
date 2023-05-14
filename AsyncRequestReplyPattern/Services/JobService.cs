namespace AsyncRequestReplyPattern.Services;

public interface IJobService
{
    void Handle(Guid jobId);
}

public class JobService: IJobService
{
    public void Handle(Guid jobId)
    {
    }

}


