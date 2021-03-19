using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Interfaces.Kafka
{
    public interface IKafkaService
    {
        /// <summary>
        /// 发送消息至指定主题
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="topicName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task PublishAsync<TMessage>(string topicName, TMessage message) where TMessage : class;

        /// <summary>
        /// 从指定主题订阅消息
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="topics"></param>
        /// <param name="messageFunc"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SubscribeAsync<TMessage>(IEnumerable<string> topics, Action<TMessage> messageFunc, CancellationToken cancellationToken) where TMessage : class;
    }
}
