namespace FAN.RabbitMQ.Topology
{
    public class Exchange : IExchange
    {
        public string Name { get; private set; }

        public Exchange(string name, string type)
        {
            Preconditions.CheckNotNull(name, "name");
            this.Name = name;
            this.Type = type;
        }

        public string Type { get; private set; }
    }
}