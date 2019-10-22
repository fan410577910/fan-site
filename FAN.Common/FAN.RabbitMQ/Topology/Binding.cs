﻿namespace FAN.RabbitMQ.Topology
{
    public class Binding : IBinding
    {
        public Binding(IBindable bindable, IExchange exchange, string routingKey)
        {
            Preconditions.CheckNotNull(bindable, "bindable");
            Preconditions.CheckNotNull(exchange, "exchange");
            Preconditions.CheckNotNull(routingKey, "routingKey");

            this.Bindable = bindable;
            this.Exchange = exchange;
            this.RoutingKey = routingKey;
        }

        public IBindable Bindable { get; private set; }
        public IExchange Exchange { get; private set; }
        public string RoutingKey { get; private set; }
    }
}