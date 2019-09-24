namespace FAN.RabbitMQ.Topology
{
    /// <summary>
    /// 表示一个 AMQP queue
    /// </summary>
    public interface IQueue : IBindable
    {
        /// <summary>
        /// 队列的名字
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 是否创建短暂的消费者，true表示创建短暂的消费者，false表示创建持久的消费者。
        /// </summary>
        bool IsExclusive { get; }
    }
}