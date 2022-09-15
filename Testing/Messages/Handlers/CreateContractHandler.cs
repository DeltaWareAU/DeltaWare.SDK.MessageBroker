using DeltaWare.SDK.MessageBroker.Extensions.Gates;
using DeltaWare.SDK.MessageBroker.Extensions.Gates.Provider;
using DeltaWare.SDK.MessageBroker.Processors;

namespace Testing.Messages.Handlers
{
    public class CreateContractHandler : MessageHandler<CreateContractMessage>
    {
        private readonly ILogger _logger;

        private readonly IMessageGateProvider _gateProvider;

        public CreateContractHandler(IMessageGateProvider gateProvider, ILogger<CreateContractHandler> logger)
        {
            _gateProvider = gateProvider;
            _logger = logger;
        }

        protected override async ValueTask ProcessAsync(CreateContractMessage message)
        {
            ContractIdSavedMessage contractSavedEvent = new ContractIdSavedMessage
            {
                ContractId = message.ContractId
            };

            _logger.LogInformation("Creating Contract: {ContractId} for User {FirstName} {LastName}", message.ContractId, message.FirstName, message.LastName);

            // Initiate an Event Gate.
            using MessageGate messageGate = _gateProvider.InitiateGate(contractSavedEvent);

            _logger.LogInformation("Contract: {ContractId} Awaiting Save Event.", message.ContractId);

            // Await the Event Gate.
            await messageGate.WaitAsync(TimeSpan.FromSeconds(60));

            _logger.LogInformation("Contract: {ContractId} has been Saved.", message.ContractId);
        }
    }
}
