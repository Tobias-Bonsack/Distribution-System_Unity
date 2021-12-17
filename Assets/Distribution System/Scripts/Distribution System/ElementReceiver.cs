using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistributionSystem
{
    public class ElementReceiver : AElementReceiver
    {
        void Awake()
        {
            SetAbstractEvents(_type);
        }
        protected override void ExtendEnterTrigger(DistributeReceiver.OnReceiveElementArgs e)
        {
            //TODO place for more general enter effect
        }
        protected override void ExtendStayTrigger(DistributeReceiver.OnReceiveElementArgs e)
        {
            //TODO place for more general stay effect
        }
        protected override void ExtendExitTrigger(DistributeReceiver.OnReceiveElementArgs e)
        {
            //TODO place for more general exit effect
        }

    }
}
