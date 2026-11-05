using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Allignment")]
public class AllignmentBehavior : FlockBehavior
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbours, main current aligment
        if (context.Count == 0)
            return agent.transform.forward;

        Vector3 alligmentMove = Vector3.zero;
        foreach (Transform item in context)
        {
            alligmentMove += item.transform.forward;
        }
        alligmentMove /= context.Count;
        alligmentMove.y = 0f;

        return alligmentMove;
    }
}
