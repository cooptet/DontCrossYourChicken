using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidence")]
public class AvoidenceBegavior : FlockBehavior
{
        public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
        {
            //if no neighbours, return no adjustments
            if (context.Count == 0)
                return Vector3.zero;

            Vector3 avoidenceMove = Vector3.zero;
            int nAvoid = 0;
            foreach (Transform item in context)
            {
                if (Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
                {
                    nAvoid++;
                    avoidenceMove += agent.transform.position - item.position; 
                }
            }
            if (nAvoid > 0)
            {
                avoidenceMove /= nAvoid;
            }
            return avoidenceMove;

    }
}
