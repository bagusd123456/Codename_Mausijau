using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Example/Look At Service")]
    public class LookAtService : Service
    {
        public CharacterMovement characterMovement;
        public TransformReference targetLookPos;
        public override void Task()
        {
            if (targetLookPos.Value != null && characterMovement != null)
                characterMovement.LookToTarget(targetLookPos.Value.position);
        }

        public override bool IsValid()
        {
            return !(targetLookPos.isInvalid || characterMovement == null);
        }
    }
}
