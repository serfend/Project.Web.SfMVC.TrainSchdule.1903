using BLL.Interfaces.Kafka;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Services.Kafka
{
    //public class KafkaService : IKafkaService
    //{
    //    public async Task PublishAsync<TMessage>(string topicName, TMessage message) where TMessage : class
    //    {
    //        var config = new ProducerConfig
    //        {
    //            BootstrapServers = "127.0.0.1:9092"
    //        };
    //        using var producer = new ProducerBuilder<string, string>(config).Build();
    //        await producer.ProduceAsync(topicName, new Message<string, string>
    //        {
    //            Key = Guid.NewGuid().ToString(),
    //            Value = message.SerializeToJson()
    //        }); ;
    //    }

    //    public async Task SubscribeAsync<TMessage>(IEnumerable<string> topics, Action<TMessage> messageFunc, CancellationToken cancellationToken) where TMessage : class
    //    {
    //        var config = new ConsumerConfig
    //        {
    //            BootstrapServers = "127.0.0.1:9092",
    //            GroupId = "crow-consumer",
    //            EnableAutoCommit = false,
    //            StatisticsIntervalMs = 5000,
    //            SessionTimeoutMs = 6000,
    //            AutoOffsetReset = AutoOffsetReset.Earliest,
    //            EnablePartitionEof = true
    //        };
    //        //const int commitPeriod = 5;
    //        using var consumer = new ConsumerBuilder<Ignore, string>(config)
    //                   .SetErrorHandler((_, e) =>
    //                   {
    //                       Console.WriteLine($"Error: {e.Reason}");
    //                   })
    //                   .SetStatisticsHandler((_, json) =>
    //                   {
    //                       Console.WriteLine($" - {DateTime.Now:yyyy-MM-dd HH:mm:ss} > 消息监听中..");
    //                   })
    //                   .SetPartitionsAssignedHandler((c, partitions) =>
    //                   {
    //                       string partitionsStr = string.Join(", ", partitions);
    //                       Console.WriteLine($" - 分配的 kafka 分区: {partitionsStr}");
    //                   })
    //                   .SetPartitionsRevokedHandler((c, partitions) =>
    //                   {
    //                       string partitionsStr = string.Join(", ", partitions);
    //                       Console.WriteLine($" - 回收了 kafka 的分区: {partitionsStr}");
    //                   })
    //                   .Build();
    //        consumer.Subscribe(topics);
    //        try
    //        {
    //            while (true)
    //            {
    //                try
    //                {
    //                    var consumeResult = consumer.Consume(cancellationToken);
    //                    Console.WriteLine($"Consumed message '{consumeResult.Message?.Value}' at: '{consumeResult?.TopicPartitionOffset}'.");
    //                    if (consumeResult.IsPartitionEOF)
    //                    {
    //                        Console.WriteLine($" - {DateTime.Now:yyyy-MM-dd HH:mm:ss} 已经到底了：{consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");
    //                        continue;
    //                    }
    //                    TMessage messageResult = null;
    //                    try
    //                    {
    //                        messageResult = JsonConvert.DeserializeObject<TMessage>(consumeResult.Message.Value);
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        var errorMessage = $" - {DateTime.Now:yyyy-MM-dd HH:mm:ss}【Exception 消息反序列化失败，Value：{consumeResult.Message.Value}】 ：{ex.StackTrace?.ToString()}";
    //                        Console.WriteLine(errorMessage);
    //                        messageResult = null;
    //                    }
    //                    if (messageResult != null/* && consumeResult.Offset % commitPeriod == 0*/)
    //                    {
    //                        messageFunc(messageResult);
    //                        try
    //                        {
    //                            consumer.Commit(consumeResult);
    //                        }
    //                        catch (KafkaException e)
    //                        {
    //                            Console.WriteLine(e.Message);
    //                        }
    //                    }
    //                }
    //                catch (ConsumeException e)
    //                {
    //                    Console.WriteLine($"Consume error: {e.Error.Reason}");
    //                }
    //            }
    //        }
    //        catch (OperationCanceledException)
    //        {
    //            Console.WriteLine("Closing consumer.");
    //            consumer.Close();
    //        }
    //        await Task.CompletedTask;
    //    }
    //}
}
