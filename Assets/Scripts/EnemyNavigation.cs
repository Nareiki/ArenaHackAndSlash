using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] private Transform _destination;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.destination = _destination.position;
        StartCoroutine(FollowPlayer());
    }

    private IEnumerator FollowPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Debug.Log("Follow Player");
            float distance = (_destination.position - transform.position).magnitude;
            switch (distance)
            {
                case var _ when distance > 10:
                    agent.destination = _destination.position;
                    break;
                case var _ when distance < 10 && distance > 3:
                    agent.destination = _destination.position / 2;
                    break;
                case var _ when distance <= 3:
                    Debug.Log("Attack");
                    break;

                default:
                    Debug.Log("Test");
                    break;
            }

        }
    }
}
