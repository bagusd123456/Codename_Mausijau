using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public ParticleSystem WalkDecal;

    Camera mainCam;
    public RTSCameraTargetController cameraTargetController;

    public PointController currentUnit;
    public Transform currentSelected;

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
            if (currentSelected != hit.transform)
            {
                if (currentSelected != null && currentSelected != hit.transform)
                {
                    currentSelected.GetComponent<Outline>().enabled = false;
                    currentSelected = null;
                }
            }
            if (hit.transform.tag == "Platform")
            {
                if (currentSelected != hit.transform)
                {
                    currentSelected = hit.transform;
                    currentSelected.GetComponent<Outline>().enabled = true;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    //if(hit.transform.gameObject.GetComponent<MoveToRandomPosition>() == null)
                    //{
                    //    cameraTargetController.LockOnTarget(hit.transform.position, 10);
                    //}
                    //else
                    //{
                    //    cameraTargetController.LockOnTarget(hit.transform, 20, true);
                    //}
                    currentTargetPosition = hit.transform;
                    currentUnit.MoveParentTo(hit.transform.position);

                    //Show the walk decal
                    //WalkDecal.transform.position = _moveTarget.WithNewY(0.1f);
                    WalkDecal.transform.position = currentTargetPosition.position + Vector3.up * 0.05f;
                    WalkDecal.Play();
                }
            }
        }

    }
}
