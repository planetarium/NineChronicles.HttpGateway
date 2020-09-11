namespace NineChronicles.HttpGateway.Controllers
{
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Bencodex;
    using Bencodex.Types;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Nekoyume;
    using Nekoyume.Model.State;
    using Nekoyume.Shared.Services;

    [ApiController]
    [Route("[controller]")]
    public class BalanceController : ControllerBase
    {
        public BalanceController(ILogger<BalanceController> logger, IBlockChainService blockChainService)
        {
            this.Logger = logger;
            this.BlockChainService = blockChainService;
        }

        private ILogger<BalanceController> Logger { get; }

        private IBlockChainService BlockChainService { get; }

        [HttpGet("{currency}/{address}")]
        public async Task<ContentResult> Get(string currency, string address)
        {
            var codec = new Codec();
            this.Logger.LogInformation("currency:" + currency);
            this.Logger.LogInformation("address:" + address);
            var goldCurrencyStateBytes = await this.BlockChainService.GetState(GoldCurrencyState.Address.ToByteArray());
            var goldCurrency = new GoldCurrencyState(
                (Dictionary)codec.Decode(goldCurrencyStateBytes)).Currency;
            var bytes = await this.BlockChainService.GetBalance(this.ParseHex(address), codec.Encode(goldCurrency.Serialize()));
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
