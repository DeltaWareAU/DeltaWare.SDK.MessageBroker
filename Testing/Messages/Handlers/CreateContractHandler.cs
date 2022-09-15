using DeltaWare.SDK.MessageBroker.Extensions.Gates;
using DeltaWare.SDK.MessageBroker.Extensions.Gates.Provider;
using DeltaWare.SDK.MessageBroker.Processors;

namespace Testing.Messages.Handlers
{
    public class CreateContractHandler : MessageHandler<CreateContractMessage>
    {
        private readonly ILogger _logger;

        private readonly IEventGateProvider _gateProvider;

        public CreateContractHandler(IEventGateProvider gateProvider, ILogger<CreateContractHandler> logger)
        {
            _gateProvider = gateProvider;
            _logger = logger;
        }

        protected override async ValueTask ProcessAsync(CreateContractMessage message)
        {
            _logger.LogInformation("Creating Contract: {ContractId} for User {FirstName} {LastName}", message.ContractId, message.FirstName, message.LastName);

            using EventGate eventGate = _gateProvider.GetGate(new ContractIdSavedMessage
            {
                ContractId = message.ContractId
            });

            _logger.LogInformation("Contract: {ContractId} Awaiting Saved.", message.ContractId);

            await eventGate.WaitAsync();

            _logger.LogInformation("Contract: {ContractId} has been Saved.", message.ContractId);
        }
    }
}
