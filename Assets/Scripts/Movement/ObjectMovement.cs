using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    void Update()
    {
        if (GameManager._canMove) // Start & Stop
        {
            transform.position -= new Vector3(0f, 0f, GameManager._worldSpeed * Time.deltaTime); //Remove 0 from X, 0 from Y and movespeed * time from Z axis.
        }

        if (transform.position.z < GameObject.Find("Deletion Threshold").transform.position.z) //If an object reach the deletion pos.
        {
            if (gameObject.tag.Equals("Path"))
            {
                transform.position = GameObject.Find("Generation Threshold").transform.position ; //Reuse the generated path objects by repositioning them.
            }
            else if (gameObject.tag.Equals("Coins"))
            {
                int random = Random.Range(1, 16); //to prevent the collision between new generated objects and reused ones
                transform.position = GameObject.Find("Generation Threshold").transform.position * random;

                for (int i = 0; i < transform.childCount; i++) //if a coin's child is collected, setActive(false) of its child.
                {
                    transform.GetChild(i).gameObject.SetActive(true); //Before it is repositioning, active its all childs to make it collactable again.
                }
            }
            else if (gameObject.tag.Equals("Obstacles"))
            {
                int random = Random.Range(1, 16); //to prevent the collision between new generated objects and reused ones
                transform.position = GameObject.Find("Generation Threshold").transform.position;
                gameObject.SetActive(false);
            }
            else if (gameObject.tag.Equals("Background Objects"))
            {
                int random = Random.Range(30, 60); //to prevent the collision between new generated objects and reused ones
                transform.position = new Vector3(transform.position.x, 0f, random);
            }
            else if (gameObject.tag.Equals("Tree"))
            {
                transform.position += Vector3.forward * 50f; //Repositioning the trees
            }
            else 
            {
                Destroy(gameObject);
            }
        }
    }
}
