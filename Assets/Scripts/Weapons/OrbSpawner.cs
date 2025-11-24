using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class OrbSpawner : MonoBehaviour
{
    public GameObject orb;
    public GameObject playerpaddle;
    // Start is called before the first frame update
 
    void Start()
    {
        //Vector2 rand_position = RandPosition();
        //Vector2 playerPosition = playerpaddle.transform.position;
        //GameObject orbClone;
        //orbClone = Instantiate(orb, playerPosition , this.transform.rotation) as GameObject;
        //orbClone.transform.SetParent(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (transform.childCount == 0 && context.performed)
        {
            Vector2 playerPosition = playerpaddle.transform.position;
            GameObject orbClone;
            orbClone = Instantiate(orb, playerPosition,
            this.transform.rotation) as GameObject;
            orbClone.transform.SetParent(this.transform);
        }
    }

    private Vector2 RandPosition()
    {
        // Declare the possible positions
        Vector2 position1 = new Vector2(-13.5f, 11.5f);
        Vector2 position2 = new Vector2(-8.5f, 12.5f);
        Vector2 position3 = new Vector2(-3.5f, 11.5f);

        // Generate a random number between 1 and 3 (inclusive)
        int rand = Random.Range(1, 4);

        // Return the position based on the random number
        if (rand == 1)
        {
            return position1;
        }
        else if (rand == 2)
        {
            return position2;
        }
        else
        {
            return position3;
        }
    }


}
