using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectSelector : MonoBehaviour
{
    public static Action<Transform> OnTargetSelected;
    public ParticleSystem WalkDecal;

    Camera mainCam;
    public RTSCameraTargetController cameraTargetController;

    public PointController currentUnit;
    public Transform hoveredTransform;
    public Transform selectedTransform;
    public List<UnitCondition> currentSelectedArmy = new List<UnitCondition>();

    public static Transform currentTargetPosition;
    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //If the raycast hits a transform try to get the outline component and enable it
            if (hit.transform != null)
            {
                var lastHovered = hoveredTransform;

                hoveredTransform = hit.transform;
                if (hit.transform.TryGetComponent(out Outline unitOutline))
                    unitOutline.enabled = true;

                //Check if hovered transform is not selected in group
                bool isGroupSelected = false;
                if(lastHovered != null)
                    if (lastHovered.transform.TryGetComponent(out UnitCondition unit))
                        isGroupSelected = currentSelectedArmy.Contains(unit);

                if (lastHovered != null && lastHovered != hoveredTransform && !isGroupSelected)
                {
                    if(lastHovered.transform.TryGetComponent(out Outline lastHoveredUnitOutline))
                    {
                        lastHoveredUnitOutline.enabled = false;
                    }
                }
            }

            //If the player clicks on a target, register the target as selected
            if (Input.GetMouseButtonDown(0))
            {
                //If the target is a platform, lock on to the platform
                if (hit.transform.CompareTag("Platform"))
                {
                    OnTargetSelected?.Invoke(hit.transform);
                    selectedTransform = hit.transform;
                    WalkDecal.transform.position = selectedTransform.position + Vector3.up * 0.05f;
                    WalkDecal.Play();
                }

                //Check if the target is a unit
                else
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("AlliedUnit"))
                    {
                        ResetSelectedArmy();
                        if (hit.transform != null)
                        {
                            SelectUnitArmy(hit.transform);
                        }
                    }
                    
                    //If not a unit, reset the selected transform
                    else
                    {
                        ResetSelectedArmy();
                    }
                }
            }
        }
    }

    public void LockOnTarget()
    {
        ////Lock on to the target
        //if(hit.transform.gameObject.GetComponent<MoveToRandomPosition>() == null)
        //{
        //    cameraTargetController.LockOnTarget(hit.transform.position, 10);
        //}
        //else
        //{
        //    cameraTargetController.LockOnTarget(hit.transform, 20, true);
        //}
    }

    public void HandleSelectionUnit()
    {

    }

    public void SelectUnitArmy(Transform target)
    {
        selectedTransform = target.transform;
        currentSelectedArmy = target.transform.GetComponent<UnitCondition>().unitArmy;

        if (currentSelectedArmy.Count > 0)
        {
            foreach (var selectedUnit in currentSelectedArmy)
            {
                selectedUnit.isSelected = true;

                if (selectedUnit.TryGetComponent(out Outline unitOutline))
                    unitOutline.enabled = true;
            }
        }
    }

    public void ResetSelectedArmy()
    {
        if (currentSelectedArmy != null && currentSelectedArmy.Count > 0)
        {
            foreach (var selectedUnit in currentSelectedArmy)
            {
                selectedUnit.isSelected = false;

                if (selectedUnit.TryGetComponent(out Outline unitOutline))
                    unitOutline.enabled = false;
            }
        }

        currentSelectedArmy = new List<UnitCondition>();
        selectedTransform = null;
        OnTargetSelected?.Invoke(null);
    }
}
