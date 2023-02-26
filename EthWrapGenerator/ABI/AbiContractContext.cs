using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.CSharp;
using Nethereum.ABI;
using Nethereum.ABI.Model;

namespace EthWrapGenerator.ABI
{
    public class AbiContractContext
    {
        public ContractModel ContractModel { get; }
        public List<AbiContractFunctionModel> CallFunctions { get;  }
        public List<AbiContractFunctionModel> SendFunctions { get;  }
        public List<AbiContractEventModel> Events { get; set; }

        public List<AbiContractStructModel> Structs => _structs.Values.ToList();
        private readonly ContractABI _contractAbi;
        private static readonly CSharpCodeProvider _codeProvider = new CSharpCodeProvider();
        private readonly Dictionary<string, AbiContractStructModel> _structs;
        private int _unknownCount;
        public AbiContractContext(string folder, string contract, ContractABI contractAbi)
        {
            ContractModel = new ContractModel()
            {
                Namespace = folder,
                ContractName = contract
            };
        
            _contractAbi = contractAbi;
            _structs = new Dictionary<string, AbiContractStructModel>();
            CallFunctions = new List<AbiContractFunctionModel>();
            SendFunctions = new List<AbiContractFunctionModel>();
            Events = new List<AbiContractEventModel>();
            PrepareTupleTypes();
            PrepareFunctions(true, CallFunctions);
            PrepareFunctions(false, SendFunctions);
            PrepareEvents();
        }
        private void PrepareEvents()
        {

            foreach (var eventModel in _contractAbi.Events)
            {
                
                Events.Add(new AbiContractEventModel()
                {
                    Name = eventModel.Name,
                    SystemName = GetValidPropertyName(eventModel.Name) + eventModel.Sha3Signature.Substring(0,8),
                    Parameters = eventModel.InputParameters.Select(t => new AbiContractParameterModel()
                    {
                        Name = GetValidPropertyName(t.Name),
                        Order = t.Order,
                        Indexed = t.Indexed,
                        Type = t.Type,
                        SystemType = GetSystemTypeName(t.ABIType, t.InternalType)
                    }).ToArray(),
                });
            }
            
        }
        
        private void PrepareFunctions(bool isConstants, List<AbiContractFunctionModel> source)
        {
            var constFunctions = _contractAbi.Functions.Where(t => t.Constant == isConstants).ToList();
            foreach (var function in constFunctions)
            {
                var needRequest = function.InputParameters.Any();
                
                source.Add(new AbiContractFunctionModel()
                {
                    Name = function.Name,
                    ShaSignature = function.Sha3Signature.ToLower(),
                    SystemName = GetValidPropertyName(function.Name) + function.Sha3Signature,
                    NeedRequest = needRequest,
                    RequestModel = function.InputParameters.Select(t => new AbiContractParameterModel()
                    {
                        Name = GetValidPropertyName(t.Name),
                        Order = t.Order,
                        Indexed = t.Indexed,
                        Type = t.Type,
                        SystemType = GetSystemTypeName(t.ABIType, t.InternalType)
                    }).ToArray(),
                    ResponseModel = function.OutputParameters.Select(t => new AbiContractParameterModel()
                    {
                        Name = GetValidPropertyName(t.Name),
                        Order = t.Order,
                        Indexed = t.Indexed,
                        Type = t.Type,
                        SystemType = GetSystemTypeName(t.ABIType, t.InternalType)
                    }).ToArray()
                    
                });
            }
            
        }
        
        private void PrepareTupleTypes()
        {
            var allParameters = _contractAbi.Functions
                .SelectMany(t => t.InputParameters.Union(t.OutputParameters)).ToList();
            
            var possibleTupleParameters =  allParameters
                .Where(t => t.ABIType is ArrayType || t.ABIType is TupleType).ToList();
            foreach (var parameter in possibleTupleParameters)
            {
                PrepareNestedTypes(parameter.ABIType, parameter.InternalType);
            }
        }

        private void PrepareNestedTypes(ABIType type, string internalType)
        {
            switch (type)
            {
                case ArrayType arrayType:
                    PrepareNestedTypes(arrayType.ElementType,
                        internalType.Substring(0, internalType.Length - 2));
                    break;
                case TupleType tuple:
                    var structName = StructToSystemName(internalType);

                    if (_structs.ContainsKey(structName))
                    {
                        return;
                    }
                    
                    var components = tuple.Components;

                    foreach (var component in components)
                    {
                        PrepareNestedTypes(component.ABIType, component.InternalType);
                    }

                    _structs[structName] = new AbiContractStructModel()
                    {
                        Name = structName,
                        Parameters = components.Select(t => new AbiContractParameterModel
                        {
                            Indexed = t.Indexed,
                            Name = GetValidPropertyName(t.Name),
                            Order = t.Order,
                            Type = t.Type,
                            SystemType = GetSystemTypeName(t.ABIType, t.InternalType)
                        }).ToArray()
                    };
                    break;
            }
        }

        private string GetValidPropertyName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return $"Unknown{_unknownCount++}";
            }

            var upperName = string.Concat(char.ToUpper(name[0]), name.Substring(1));
            return !_codeProvider.IsValidIdentifier(upperName) ? _codeProvider.CreateValidIdentifier(upperName) : upperName;
        }
        
        private string GetSystemTypeName(ABIType abiType, string internalType)
        {
            switch (abiType)
            {
                case AddressType _:
                case StringType _:
                    return "string";
                case IntType _:
                    return "BigInteger";
                case BoolType _:
                    return "bool";
                case BytesType _:
                case Bytes32Type _:
                case BytesElementaryType _:
                    return "byte[]";
                case ArrayType arrayType:
                    return
                        $"List<{GetSystemTypeName(arrayType.ElementType, internalType.Substring(0, internalType.Length - 2))}>";
                case TupleType _:
                    return StructToSystemName(internalType);
                default:
                    throw new InvalidEnumArgumentException($"unknown type to parse abi to system {abiType}");
            }
        }

        private static string StructToSystemName(string name)
        {
            return name.Replace("struct ", "").Replace(".", "_");
        }
    }
}