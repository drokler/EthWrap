using System.Numerics;
using biswapAuction;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

var solAuction = new SolidityAuction();

solAuction
    .Configure()
    .Load_canReceive()
    .LoadProlongationTime();

var web3 = new Web3("http://192.168.30.4:8545");
;

var block = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(BlockParameter.CreateLatest());
var list = new List<TransactionReceipt>();
foreach (var transaction in block.Transactions)
{
    var receipts = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transaction.TransactionHash);
    list.Add(receipts);
}


[Event("Buy")]
public class Buy89f5adc1Event : IEventDTO
{
    [Parameter("address", null, 1, true)]
    public string From { get; set; }

    [Parameter("address", null, 2, true)]
    public string To { get; set; }

    [Parameter("uint256", null, 3, false)]
    public BigInteger NftId { get; set; }

    [Parameter("uint256", null, 4, false)]
    public BigInteger Price { get; set; }
    
    
    
    public static Buy89f5adc1Event FromInput(Web3 web3)
    {
        var handler = web3.Eth.GetEvent<Buy89f5adc1Event>("");
        var events = handler.DecodeAllEventsForEvent(Array.Empty<FilterLog>());
        return null;
    }
}

;
//
//
// using System.Numerics;
// using Era7;
// using Nethereum.ABI.FunctionEncoding.Attributes;
// using Nethereum.BlockchainProcessing.Processor;
// using Nethereum.Contracts;
// using Nethereum.RPC.Eth.DTOs;
// using Nethereum.Web3;
//
//
// var web3 = new Web3("http://192.168.30.4:8545");
// var test = new AbiCard(web3, "0x07D971C03553011a48E951a53F48632D37652Ba1" );
//
// var res = await test.GetAllFunctions(25849379, 25849417);
// ;
//
// var outPut = new AbiCard.GetPlayerCardsad5c21d9Request();
// var builder = new FunctionBuilder<AbiCard.GetPlayerCardsad5c21d9Request>(null);
// outPut = builder.DecodeFunctionInput(outPut, "tx");
// var req = new AbiCard.GetPlayerCardsad5c21d9Request()
// {
//     Player = "0x311CD689C3e362165F55c7987A99BBB1ED303BA2"
// };
//
//
//
// var cards = await test.GetPlayerCardsad5c21d9(req);
//
// Console.Write(cards);
//
// [Event("Transfer")]
// public class TransferEventDTO : IEventDTO
// {
//     [Parameter("address", "_from", 1, true)]
//     public string From { get; set; }
//
//     [Parameter("address", "_to", 2, true)]
//     public string To { get; set; }
//
//     [Parameter("uint256", "_value", 3, false)]
//     public BigInteger Value { get; set; }
// }
//
// public class Test
// {
//     private Web3 _web3;
//     private static Dictionary<string, FunctionBuilderBase> _builders = new()
//     {
//         { "card",  new FunctionBuilder<AbiCard.GetPlayerCardsad5c21d9Request>(null)}
//     };
//     
//     private static Dictionary<string, Func<string, FunctionMessage>> _buildAction = new()
//     {
//         { "card", (str) =>
//             {
//                 var builder = (FunctionBuilder<AbiCard.GetPlayerCardsad5c21d9Request>)_builders["card"];
//                 return builder.DecodeFunctionInput(new AbiCard.GetPlayerCardsad5c21d9Request(), str);
//             }
//         },
//     };
//
//     public async Task<List<FunctionMessage>> GetAllFunctions(ulong blockBefore, CancellationToken cancellationToken = default)
//     {
//         var ret = new List<FunctionMessage>();
//         var toBlock = (ulong) (await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync()).Value;
//         var fromBlock = toBlock - blockBefore;
//         
//         var processor = _web3.Processing.Blocks.CreateBlockProcessor(steps =>
//         {
//             steps.TransactionStep.SetMatchCriteria(t => t.Transaction.IsTo(""));
//             steps.TransactionStep.AddSynchronousProcessorHandler(t =>
//             {
//                 var txHex = t.Transaction.Input.Substring(2, 8);
//                 ret.Add(_buildAction[txHex](t.Transaction.Input));
//             });
//
//         });
//         await processor.ExecuteAsync(new BigInteger(toBlock), cancellationToken, new BigInteger(fromBlock));
//
//         return ret;
//     }
// }
