using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;
using UnityEngine.Serialization;

namespace MBTExample
{
    [AddComponentMenu("")]
    [MBTNode("Example/Player Command Service")]
    public class PlayerCommandService : Service
    {
        private ObjectSelector objectSelector;
        
        private Transform target;

        public TransformReference selfCondition = new TransformReference(VarRefMode.DisableConstant);
        public TransformReference variableToSet = new TransformReference(VarRefMode.DisableConstant);
        public BoolReference onCommandToSet = new BoolReference(VarRefMode.DisableConstant);
        public TransformReference targetToReset = new TransformReference(VarRefMode.DisableConstant);

        public void OnEnable()
        {
            ObjectSelector.OnTargetSelected += OnTargetSelected;
        }

        public void OnDisable()
        {
            ObjectSelector.OnTargetSelected -= OnTargetSelected;
        }

        private void OnTargetSelected(Transform targetTransform)
        {
            var currentUnit = selfCondition.Value.GetComponent<UnitCondition>();
            if (!currentUnit.isDead && currentUnit.isSelected)
            {
                target = targetTransform;
                onCommandToSet.Value = true;
            }
        }

        public override void Task()
        {
            if (objectSelector == null)
            {
                try
                {
                    objectSelector = FindObjectOfType<ObjectSelector>();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Cannot Find 'ObjectSelector' script..." +
                                   $"\n Please Assign one in GameEnvironment... " +
                                   $"\n Error Code: {e}");
                    throw;
                }
            }

            if (target != null)
            {

                variableToSet.Value = target;
                //onCommandToSet.Value = true;

                //targetReset.Value = null;
            }
            else
            {
                variableToSet.Value = null;
                onCommandToSet.Value = false;
            }
        }
    }
}

