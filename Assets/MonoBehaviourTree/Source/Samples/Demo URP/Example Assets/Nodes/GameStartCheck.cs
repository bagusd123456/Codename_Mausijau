using System.Collections;
using System.Collections.Generic;
using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode("Shiro/Game Start Check")]
public class GameStartCheck : Service
{
    public BoolReference gameStartedBool;

    public override void Task()
    {
        if (GameManager.Instance == null)
        {
            //GameManager is not initialized, force AI to start
            gameStartedBool.Value = true;
        }

        //GameManager is initialized, check if game is started
        gameStartedBool.Value = GameManager.Instance.gameState == GameManager.Condition.Running;
    }
}
