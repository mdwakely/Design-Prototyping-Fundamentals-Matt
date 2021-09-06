using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameObject gameManager;


    private void Start()
    {
        gameManager = GameObject.Find("Game Manager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Debug.Log("Goal");
            gameManager.GetComponent<Manager>().ResetLevel();
            
        }
           
    }


    
}
