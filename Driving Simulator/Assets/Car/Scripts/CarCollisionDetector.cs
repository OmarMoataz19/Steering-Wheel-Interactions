using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollisionDetector : MonoBehaviour
{
    private int collisionCount = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Barrier"))
        {
            collisionCount++;
            Debug.Log("Collision count: " + collisionCount);
        }
    }
}
