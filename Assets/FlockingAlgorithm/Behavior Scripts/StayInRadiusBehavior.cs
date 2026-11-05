using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Flock/Behavior/Stay In Radius")]
public class StayInRadiusBehavior : FlockBehavior
{
    public Vector3 center;
    public float radius;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector3 centerOffset = center - agent.transform.position;
        float t = centerOffset.magnitude / radius;
        //f by³o 0.9 -> trochê za szybko id¹ do œrodka
        if (t < 0.95f)
        {
            return Vector3.zero;
        }

        return centerOffset * t * t;
    }
}
