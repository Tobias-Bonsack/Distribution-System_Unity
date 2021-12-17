using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistributionSystem
{
    public class GraphConnector : GraphMember
    {
        protected override void Awake()
        {
            _chemistryReceiver = _elementReceiver.DistributionReceiver;

            GraphSystem.AddBaseGraph(_originalGraph);
            GraphSystem.graphs[_originalGraph].Add(this);

            switch (_type)
            {
                case IDistribute.ChemistryTypes.ELECTRICITY:
                    _chemistryReceiver._onReceiveElectricity += EnterTrigger;
                    _chemistryReceiver._onReceiveElectricity += ExitTrigger;
                    break;
                default:
                    break;
            }
        }

        new protected void EnterTrigger(object sender, DistributeReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IDistributeReceiver.Status.ENTER)
            {
                if (sender is GraphMemberEmitter)
                { // Fusion of graphen
                    GraphMemberEmitter emitter = (GraphMemberEmitter)sender;
                    string emitterGraphName = emitter.GRAPH_NAME;
                    string graphName = _originalGraph;

                    if (emitterGraphName.Equals(graphName)) return;

                    //Valid Fusion
                    GraphSystem.AddCombineGraph(new string[] { graphName, emitterGraphName }, emitter.GetHashCode());
                }
                else
                { // is PowerSource
                    UpdateAbleToReceive(+1);
                }

                NewMethod();
            }
        }
        new protected void ExitTrigger(object sender, DistributeReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IDistributeReceiver.Status.EXIT)
            {
                if (sender is GraphMemberEmitter)
                { // Defusion of graphen
                    GraphMemberEmitter emitter = (GraphMemberEmitter)sender;
                    GraphSystem.RemoveCombineGraph(_originalGraph + ":" + emitter.GRAPH_NAME, emitter.GetHashCode());
                }
                else
                { // is PowerSource
                    UpdateAbleToReceive(-1);
                }

                NewMethod();
            }
        }
    }
}