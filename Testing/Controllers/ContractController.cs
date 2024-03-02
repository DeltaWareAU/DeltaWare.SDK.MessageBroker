using DeltaWare.SDK.MessageBroker.Abstractions.Publisher;
using Microsoft.AspNetCore.Mvc;
using Testing.Messages;

namespace Testing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IMessagePublisher _messagePublisher;

        public ContractController(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateContractAsync(CreateContractMessage createContract)
        {
            await _messagePublisher.PublishAsync(createContract);

            return Ok();
        }

        [HttpPost("SaveContractId/{contractId}")]
        public async Task<IActionResult> SaveContractIdAsync(string contractId)
        {
            await _messagePublisher.PublishAsync(new ContractIdSavedMessage
            {
                ContractId = contractId
            });

            return Ok();
        }
    }
}
