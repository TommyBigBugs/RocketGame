using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrustForce = 100f;
    [SerializeField] float mainThrustForce = 1200f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip crashSound;
    [SerializeField] float levelLoadDelay =2f;

    [SerializeField] ParticleSystem mainEngineEffect;
    [SerializeField] ParticleSystem winEffect;
    [SerializeField] ParticleSystem crashEffect;

    [SerializeField] Boolean devMode = false;

    [SerializeField]  int lastScene;
    [SerializeField]  int currentScene;
    [SerializeField]  int nextScene;
    Rigidbody rigidBody;
    AudioSource audioSource;
    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    int currentLevel = 0;


    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        SetLastScene();
        currentScene = SceneManager.GetActiveScene().buildIndex;
        nextScene = currentScene + 1;

    }
      

    // Update is called once per frame
    void Update ()
    {
      

        if (state == State.Alive)
        {
            RespondToRotateInput();
            RespondToThrustInput();
            
        }
        if (Debug.isDebugBuild)
        {
            respondtobebugkeys();
        }

      
    }

    private void respondtobebugkeys()
    {
        if (Input.GetKey(KeyCode.L) && (devMode = true))
        {
            state = State.Transcending;
            LoadNextScene();
        }

        if (Input.GetKey(KeyCode.M))
        {
            devMode = !devMode;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }        
        
        string objectTag = collision.gameObject.tag;

        if (objectTag == "Friendly") //
        {

            print("ok!");
            
        }

        else if (objectTag == "Finish") //win
        {
            PlayerWon();
        }

        else if (devMode == true) { return; }

        else //dead
        {
            PlayerDead();
        }
    }

    private void PlayerWon()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(winSound);
        winEffect.Play();
        Invoke("LoadNextScene", levelLoadDelay); //paramter
        
    }

    private void PlayerDead()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        crashEffect.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void SetLastScene()
    {
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        lastScene = (totalScenes - 1);

    }

    private void LoadNextScene()
    {
        if (state == State.Transcending && nextScene <= lastScene)
        {
            SceneManager.LoadScene(nextScene);           
        }
        else
        {
            SceneManager.LoadScene(0);            
        }

    }

    private void RespondToRotateInput()

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

    private void RespondToThrustInput()
    {               
         if (Input.GetKey(KeyCode.Space)) //thrust
        {
            ApplyThrust();
        }
        else
            {
                audioSource.Stop();
                mainEngineEffect.Stop();
        }
        
    }     

    private void ApplyThrust()
    {
        float thrustThisFrame = (mainThrustForce * Time.deltaTime);
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (audioSource.isPlaying == false)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (mainEngineEffect.isPlaying == false) {
            mainEngineEffect.Play();
        }
        
    }

 

}

