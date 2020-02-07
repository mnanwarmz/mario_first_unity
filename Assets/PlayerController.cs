using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public float moveRate;
	public float maxMoveRate;
	public float minMoveRate;
	public float jumpHeight;
	private float jumpCount;
	public AudioClip coinSFX;
	public AudioClip jumpSFX;

	private static int level = 1;
	private Animator anim;
	private SpriteRenderer sr;
	private Rigidbody2D rb;
	private AudioSource audio;


    void Start()
    {
    	sr = GetComponent<SpriteRenderer>();
    	rb = GetComponent<Rigidbody2D>();
    	anim = GetComponentInParent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
    	bool havePlayerMove = false;

    	if(sr != null && rb != null ){
     	
     		if(Input.GetKey("left")){
     			sr.flipX = true;  
     			rb.AddForce(-transform.right * moveRate);
     			havePlayerMove = true;
     		}
     		else if(Input.GetKey("right")){
     			sr.flipX = false;  
     			rb.AddForce(transform.right * moveRate);
     			havePlayerMove = true;
    		}
 
    		if(Input.GetKeyDown("space") && rb.velocity.y == 0){
    			rb.AddForce(transform.up * jumpHeight);
				audio.PlayOneShot(jumpSFX);
    		}

    		if(rb.velocity.x > maxMoveRate)
    			rb.velocity = new Vector2(maxMoveRate,rb.velocity.y);

    		if(rb.velocity.x < -maxMoveRate)
    			rb.velocity = new Vector2(-maxMoveRate, rb.velocity.y);

    		if(rb.velocity.x > 0 && rb.velocity.x < minMoveRate)
    			rb.velocity = new Vector2(0,rb.velocity.y);

    		if(rb.velocity.x < 0 && rb.velocity.x > -minMoveRate)
    			rb.velocity = new Vector2(0,rb.velocity.y);			
    	}

    	if(anim != null && rb != null){

    		if(rb.velocity.y != 0){
    		anim.SetBool("Jumping",true);
    		anim.SetBool("Walking",false);
    		}

    		else {
    		anim.SetBool("Jumping",false);
    		anim.SetBool("Walking",havePlayerMove);
    		}

    	}
	}

		void OnTriggerEnter2D (Collider2D col){
			if(col.gameObject.tag == "Coin"){
			audio.PlayOneShot(coinSFX);
			Destroy(col.gameObject);
			}
			if(col.gameObject.tag == "Finish"){
				level = 1;
				SceneManager.LoadScene("Level"+level);
			}

			if (col.gameObject.tag == "End")
			{
				level++;
				SceneManager.LoadScene("Level" + level);
			}
		}
	
}
