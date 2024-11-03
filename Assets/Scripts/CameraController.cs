using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Riferimento al trasform del giocatore
    public Vector3 offset; // Offset della posizione della camera rispetto al giocatore

    // Update viene chiamato una volta per frame
    void Update()
    {
        if (player != null)
        {
            // Imposta la posizione della camera in base alla posizione del giocatore e all'offset
            transform.position = player.position + offset;

            // Fai in modo che la telecamera guardi sempre verso il giocatore
            transform.LookAt(player);
        }
    }
}
