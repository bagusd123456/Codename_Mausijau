using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(GameManager.Instance.gameState != GameManager.Condition.Running) return;

        //Check if layer is enemy
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyUnit"))
        {
            //Check if the enemy is a unit
            if (other.gameObject.TryGetComponent<UnitCondition>(out var unit))
            {
                //Check if the enemy is not dead
                if (!unit.isDead)
                {
                    //Invoke the event
                    UnitCondition.OnEnemyArrivedBase?.Invoke(unit);
                }
            }
        }
    }
}
