using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace PodcastManager.Administration.CrossCutting.Rabbit;

public abstract class BaseRabbitListenerAdapter : IListenerAdapter, IDisposable
{
    private IConnection connection = null!;
    private IModel channel = null!;
    private ILogger logger = null!;

    public void SetConnectionFactory(IConnectionFactory connectionFactory)
    {
        connection = connectionFactory.CreateConnection();
        channel = connection.CreateModel();
    }
    public void SetLogger(ILogger logger) => this.logger = logger;

    public abstract void Listen();

    public void Dispose()
    {
        connection.Dispose();
        channel.Dispose();
        GC.SuppressFinalize(this);
    }
    
    protected void ListenTo(string queue, Func<Task> action, ushort prefetch = 30, bool isGlobal = true)
    {
        logger.Information("listening to: {Queue}", queue);
        ConfigureChannel();
        channel.BasicConsume(queue, false, ConfigureConsumer());

        async Task TryProcessMessage(BasicDeliverEventArgs basicDeliverEventArgs)
        {
            try
            {
                await ProcessMessage();
                channel.BasicAck(basicDeliverEventArgs.DeliveryTag, false);
            }
            catch (Exception e)
            {
                logger.Error(e, "Error: '{Error}' processing message {Queue}", e.Message, queue);
                channel.BasicNack(basicDeliverEventArgs.DeliveryTag, false, false);
            }
        }
        async Task ProcessMessage()
        {
            logger.Debug("Message Received for {Queue}", queue);
            await action();
        }
        void ConfigureChannel()
        {
            channel.QueueDeclare(queue, true, false, false);
            channel.BasicQos(0, prefetch, isGlobal);
        }
        EventingBasicConsumer ConfigureConsumer()
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (_, args) => await TryProcessMessage(args);
            return consumer;
        }
    }
}