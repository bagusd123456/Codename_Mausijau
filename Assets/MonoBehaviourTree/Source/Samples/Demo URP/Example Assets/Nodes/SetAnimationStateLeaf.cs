using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using MBT;
using UnityEngine;

[MBTNode("Example/Set Animation State")]
public class SetAnimationStateLeaf : Leaf
{
    public AnimationController animationController;
    public CharacterStance characterStance;
    public MovementStates movementState;
    public override NodeResult Execute()
    {
        animationController.CurrentState = movementState;
        animationController.characterStance = characterStance;
        return NodeResult.success;
    }
}
