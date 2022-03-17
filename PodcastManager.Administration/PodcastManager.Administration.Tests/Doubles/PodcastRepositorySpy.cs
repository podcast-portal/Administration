using System.Threading.Tasks;

namespace PodcastManager.Administration.Doubles;

public class PodcastRepositorySpy : PodcastRepositoryStub
{
    public SpyHelper<int[]> PublishPodcastsSpy { get; } = new();
    public override Task<long> PublishPodcasts(int[] codes)
    {
        PublishPodcastsSpy.Call(codes);
        return base.PublishPodcasts(codes);
    }
}