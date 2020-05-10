using System;
using System.Text.Json;
using System.Threading.Tasks;
using Bencodex;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nekoyume.Shared.Services;

namespace NineChronicles.HttpGateway.Controllers
{
    // FIXME: add test for this controller
    [ApiController]
    [Route("[controller]")]
    public class StateController : ControllerBase
    {
        private byte[] ParseHex(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; ++i)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }

            return bytes;
        }

        private readonly ILogger<StateController> _logger;

        private readonly IBlockChainService _blockChainService;

        public StateController(ILogger<StateController> logger, IBlockChainService blockChainService)
        {
            _logger = logger;
            _blockChainService = blockChainService;
        }

        [HttpGet("{address}")]
        public async Task<ContentResult> Get(string address)
        {
            var bytes = await _blockChainService.GetState(ParseHex(address));
            var codec = new Codec();
            var state = codec.Decode(bytes);
            return Content(JsonSerializer.Serialize(state, new JsonSerializerOptions
            {
                Converters = {new IValueConverter(),}
            }), "application/json");
        }
    }
}
