using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = GetComponentInChildren<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }
    void Rotate()
    {
        if (playerMovement.playerStats == playerStats.roll)
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, transform.position);
        float distance;

        if (ground.Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance);
            transform.LookAt(new Vector3(point.x, transform.position.y, point.z));
        }
    }
}
