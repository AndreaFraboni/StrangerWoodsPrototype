using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{

    public Transform teleportTarget; //Variable for TP position
    public GameObject player; //Variable for teleporting P

    void OnTriggerEnter(Collider other)
    {
        player.transform.position = teleportTarget.transform.position;

    }
}
