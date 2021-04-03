using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPathController : MonoBehaviour
{
    const float waypointGizmoRadius = 0.3f;
    private void OnDrawGizmos()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
            Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i))) ;
        }
    }

    public int GetNextIndex(int i)
    {
        if (i+1==transform.childCount) { return 0; }
        return i + 1;
    }

    public Vector3 GetWaypoint(int i)
    {
        return transform.GetChild(i).position;
    }
}
