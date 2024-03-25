using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using MBT;
using UnityEngine;

[MBTNode("Example/Set Animation State")]
public class SetAnimationStateLeaf : Leaf
{
    public AnimationController animationController;
    public MovementStates movementState;
    //public CharacterStance characterStance;
    public override NodeResult Execute()
    {
        animationController.CurrentState = movementState;
        //animationController.characterStance = characterStance;
        return NodeResult.success;
    }
}
