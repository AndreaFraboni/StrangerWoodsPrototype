using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FInishLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se l'oggetto in collisione ha un tag "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("PLAYER ARRIVED !!!");

            GameController.Instance.LevelFinished();
        }
    }



}
