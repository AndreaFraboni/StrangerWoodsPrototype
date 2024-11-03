using UnityEngine;

public class Attack : MonoBehaviour
{
    private float damageAmount = 10f; // Quantità di danni inflitti dall'attacco

    // Funzione chiamata quando un oggetto collidere con il personaggio
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("IL LUPO STA SUBENDO ATTACCO E DANNO !!!!");

        // Verifica se l'oggetto in collisione ha un tag "Enemy"
        if (other.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("IL LUPO STA SUBENDO ATTACCO E DANNO !!!!");
            // Ottieni il componente Health del nemico
            Enemy enemyHealth = other.gameObject.GetComponent<Enemy>();

            // Se il componente Health è presente
            if (enemyHealth != null)
            {
                // Infliggi danni al nemico
                enemyHealth.TakeDamage(damageAmount);

                // Opzionale: Puoi aggiungere effetti visivi o sonori qui
            }
        }
    }

}
