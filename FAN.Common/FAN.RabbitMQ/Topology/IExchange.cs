namespace FAN.RabbitMQ.Topology
{
    public interface IExchange : IBindable
    {
        string Name { get; }

        string Type { get; }
    }
}