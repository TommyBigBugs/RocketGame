using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrustForce = 100f;
    [SerializeField] float mainThrustForce = 1200f;

    Rigidbody rigidBody;
    AudioSource audioSource;


    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Rotate();
        Thrust();
	}
        
    void OnCollisionEnter(Collision collision)
    {
        string objectTag = collision.gameObject.tag;

        if (objectTag == "Friendly")
        {          
                print("Ok");             
            
        }
        else 
        {
                print("Dead");
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) //thrust
        {
            float thrustThisFrame = (mainThrustForce * Time.deltaTime);
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            if (audioSource.isPlaying == false)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;
        float rotationThisFrame = rcsThrustForce * Time.deltaTime;

        if (Input.GetKey(KeyCode.A)) 
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }
}

