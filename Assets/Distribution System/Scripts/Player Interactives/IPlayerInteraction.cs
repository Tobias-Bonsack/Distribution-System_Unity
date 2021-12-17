using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerInteraction
{
    public interface IPlayerInteraction
    {
        void MudChanges(bool isEnterTrigger, float[] changeArray);

        void GravityChange(bool isEnterTrigger, float gravityChange);
    }
}
