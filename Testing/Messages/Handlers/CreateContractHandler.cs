using DeltaWare.SDK.MessageBroker.Core.Handlers;
using DeltaWare.SDK.MessageBroker.Extensions.Gates;
using DeltaWare.SDK.MessageBroker.Extensions.Gates.Provider;

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

            await Task.Delay(20);


            if (messageGate.IsOpen)
            {
                _logger.LogInformation("Contract: {ContractId} has been Saved.", message.ContractId);
            }
            else
            {
                _logger.LogWarning("Contract: {ContractId} has been Cancelled.", message.ContractId);
            }
        }
    }
}
