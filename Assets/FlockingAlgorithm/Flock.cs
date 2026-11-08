using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehavior behavior;

    [Range(10, 160)]
    public int startingCount = 40;
    const float AgentDensity = 1f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;

    [Range(1f, 100f)]
    public float maxSpeed = 2f;

    [Range(1f, 10f)]
    public float neighborRadius = 3f;

    [Range(0f, 1f)]
    public float avoidenceRadiusMulitiplier = 1f;


    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;

    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    public LayerMask agentLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidenceRadiusMulitiplier * avoidenceRadiusMulitiplier;

        for (int i = 0; i < startingCount; i++)
        {
            Vector2 r = Random.insideUnitCircle * startingCount * AgentDensity;
            Vector3 spawnPos = transform.position + new Vector3(r.x, 0.2f, r.y);

            FlockAgent newAgent = Instantiate(
                agentPrefab,
                spawnPos,
                Quaternion.Euler(0f, Random.Range(0f, 360f), 0f),
                transform
            );
            newAgent.name = "Chicken " + i;
            agents.Add(newAgent);
        }

    }

    // Update is called once per frame
    void Update()
    {
        foreach(FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            Vector3 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }


    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextCollider = Physics.OverlapSphere(agent.transform.position, neighborRadius, agentLayer);
        foreach(Collider c in contextCollider)
        {
            //if its not our own collider
            if(c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
