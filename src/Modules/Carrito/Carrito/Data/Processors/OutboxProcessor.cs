using System.Text.Json;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Carrito.Data.Processors;

public class OutboxProcessor(IServiceProvider serviceProvider, IBus bus, ILogger<OutboxProcessor> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                
                // Recuperar mensajes del outbox que no han sido procesados en la bd
                var outboxMessages = await dbContext.OutboxMessages
                    .Where(m => m.ProcessedOn == null)
                    .ToListAsync(stoppingToken);

                foreach (var message in outboxMessages)
                {
                    // Deserialize the message content
                    var eventType = Type.GetType(message.Type);
                    if (eventType == null)
                    {
                        logger.LogWarning("Event type {EventType} not found for message {MessageId}", message.Type, message.Id);
                        continue;
                    }

                    var eventMessage = JsonSerializer.Deserialize(message.Content, eventType);
                    if (eventMessage == null)
                    {
                        logger.LogWarning("Failed to deserialize message {Content}", message.Content);
                        continue;
                    }

                    // Publish the event to the message bus
                    await bus.Publish(eventMessage, stoppingToken);

                    // Mark the message as processed
                    message.ProcessedOn = DateTime.UtcNow;
                    
                    logger.LogInformation("Successfully processed outbox message {MessageId} of type {EventType}", message.Id, eventType.Name);
                }

                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing outbox messages");
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // espera 10 segundos antes de procesar nuevamente y volver a comprobar la tabla Outbox
        }
    }
}