using System.Threading.Tasks;

namespace PodcastManager.Administration.Doubles;

public class PlaylistRepositorySpy : PlaylistRepositoryStub
{
    public SpyHelper ListRelatedPodcastsSpy { get; } = new();
    
    public override Task<int[]> ListRelatedPodcasts()
    {
        ListRelatedPodcastsSpy.Call();
        return base.ListRelatedPodcasts();
    }
}