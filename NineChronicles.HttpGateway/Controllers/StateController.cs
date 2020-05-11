namespace NineChronicles.HttpGateway.Controllers
{
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Bencodex;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Nekoyume.Shared.Services;

    [ApiController]
    [Route("[controller]")]
    public class StateController : ControllerBase
    {
        public StateController(ILogger<StateController> logger, IBlockChainService blockChainService)
        {
            this.Logger = logger;
            this.BlockChainService = blockChainService;
        }

        private ILogger<StateController> Logger { get; }

        private IBlockChainService BlockChainService { get; }

        [HttpGet("{address}")]
        public async Task<ContentResult> Get(string address)
        {
            var bytes = await this.BlockChainService.GetState(this.ParseHex(address));
            var codec = new Codec();
            var state = codec.Decode(bytes);
            return this.Content(
                JsonSerializer.Serialize(state, new JsonSerializerOptions
                {
                    Converters =
                    {
                        new BencodexValueConverter(),
                    },
                }), "application/json");
        }

        private byte[] ParseHex(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; ++i)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }

            return bytes;
        }
    }
}
