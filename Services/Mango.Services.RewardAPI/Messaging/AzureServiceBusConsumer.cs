using Azure.Messaging.ServiceBus;
using Mango.Services.RewardAPI.Message;
using Mango.Services.RewardAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.RewardAPI.Messaging
{
	public class AzureServiceBusConsumer : IAzureServiceBusConsumer
	{
		private readonly string serviceBusConnectionString;
		private readonly string orderCreatedTopic;
		private readonly string orderCreatedRewardSubscription;
		private readonly IConfiguration _configuration;
		private readonly RewardService _rewardService;

		private ServiceBusProcessor _rewardProccesor;

		public AzureServiceBusConsumer(IConfiguration configuration, RewardService rewardService)
		{
			_rewardService = rewardService;
			_configuration = configuration;

			serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

			orderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
			orderCreatedRewardSubscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Rewards_Subscription");

			var client = new ServiceBusClient(serviceBusConnectionString);
			_rewardProccesor = client.CreateProcessor(orderCreatedTopic, orderCreatedRewardSubscription);

		}

		public async Task Start()
		{
			_rewardProccesor.ProcessMessageAsync +=  OnNewOrderRewardsRequestReceived;
			_rewardProccesor.ProcessErrorAsync += ErrorHandler;
			await _rewardProccesor.StartProcessingAsync();

		}


		public async Task Stop()
		{
			await _rewardProccesor.StopProcessingAsync();
			await _rewardProccesor.DisposeAsync();

		}

		private  async Task OnNewOrderRewardsRequestReceived(ProcessMessageEventArgs args)
		{
			var message = args.Message;
			var body = Encoding.UTF8.GetString(message.Body);

			RewardsMessage objMessage = JsonConvert.DeserializeObject<RewardsMessage>(body);
			try
			{
				//todo - email giriş dene
				await _rewardService.UpdateRewards(objMessage);
				await args.CompleteMessageAsync(args.Message);
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		private  Task ErrorHandler(ProcessErrorEventArgs args)
		{
			Console.WriteLine(args.Exception.ToString());
			return Task.CompletedTask;
		}
		
	}
}
