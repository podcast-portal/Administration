using PodcastManager.Administration.Domain.Factories;

namespace PodcastManager.Administration.CrossCutting.Rabbit;

public class RabbitAdministrationListenerAdapter : BaseRabbitListenerAdapter
{
    private IInteractorFactory interactorFactory = null!;

    public void SetInteractorFactory(IInteractorFactory interactorFactory) =>
        this.interactorFactory = interactorFactory;

    public override void Listen()
    {
        ListenTo(RabbitConfiguration.PublishAllFromPlaylistQueue,
            () => interactorFactory.CreatePodcastPublisher().PublishAllFromPlaylists());
    }
}