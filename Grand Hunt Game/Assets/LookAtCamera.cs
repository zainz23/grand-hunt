using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform trackingHelper;

    void Update()
    {
        var newX = trackingHelper.position.x;
        var newZ = trackingHelper.position.z - 17;
        var y = transform.position.y;
        var targetPosition = new Vector3(newX, y, newZ);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.01f * Time.deltaTime);
    }
}