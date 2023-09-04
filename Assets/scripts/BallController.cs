using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour {
    bool win = false;
	int score = 0;
	[SerializeField] Text label;
    [SerializeField] Text scoreLabel;

    void OnCollisionEnter(Collision collision)
    {
        if ((!win && collision.gameObject.tag == "Terrain"))
        {
            print("Game over");
			label.enabled = true;
            // Application.Quit();
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!win && other.gameObject.tag == "Finish")
        {
            print("You win");
			label.text = "You win";
			label.color = Color.green;
			label.enabled = true;
            win = true;       
        }
		if (!win && other.gameObject.tag == "Teapot")
		{
            Destroy(other);

            other
                .gameObject
                .GetComponent<Animator>()
                .Play("collect");
            
            score++;
			print (score);
            scoreLabel.text = "Score: " + score;
			Destroy (other.gameObject, 1.0f);	
		}
    }
}
