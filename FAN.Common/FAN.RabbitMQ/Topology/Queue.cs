namespace FAN.RabbitMQ.Topology
{
    public class Queue : IQueue
    {
        public Queue(string name, bool isExclusive)
        {
            this.Name = name;
            this.IsExclusive = isExclusive;
        }

        public string Name { get; private set; }
        public bool IsExclusive { get; private set; }
    }
}