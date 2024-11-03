using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{

    public static int numberOfCoins;
    public TextMeshProUGUI coinsText;


    // Start is called before the first frame update
    void Start()
    {

        numberOfCoins = 0; // Codice da inserire nello script che gestisce l'UI

    }

    // Update is called once per frame
    void Update()
    {

        coinsText.text = "Coins: " + numberOfCoins; // Codice da inserire nello script che gestisce l'UI
    }

}
