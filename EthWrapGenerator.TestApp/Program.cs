

using Era7;
using Nethereum.Web3;

var web3 = new Web3("http://192.168.30.4:8545");

var test = new AbiCard(web3, "0x07D971C03553011a48E951a53F48632D37652Ba1");

var name = await test.GetPlayerCards(new AbiCard.GetPlayerCardsRequest()
{
    Player = "0x311CD689C3e362165F55c7987A99BBB1ED303BA2"
});

Console.Write("ssdsssdfdfs");
