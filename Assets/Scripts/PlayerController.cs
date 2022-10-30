using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController myCharacterController;

    private float speed = 10;
    private float horizontalInput;
    private float forwardInput;
    private Vector3 moving;
    private Vector3 finalOffset = new Vector3(0, 0, 0);
    public bool hasSpeedPowerup;
    public bool hasDogPowerup;
    public static int newScore;

    public ParticleSystem dirtParticle;
    private AudioSource playerAudio;
    public AudioClip foodSound;
    public AudioClip powerupSound;
    public AudioClip powerdownSound;

    public GameObject powerupIndicator;

    // Start is called before the first frame update
    void Start()
    {
        myCharacterController = GetComponent<CharacterController>();
        playerAudio = GetComponent<AudioSource>();
        //anim = GetComponent<Animator>();
        var main = dirtParticle.main;
        main.simulationSpeed = 0.3f;
        newScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        Movement();
        Rotation();

        powerupIndicator.transform.position = transform.position + new Vector3(0, 1, 0);
    }

    void Movement()
    {
        if (SpawnManager.isGameActive == true)
        {
            moving = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            if (hasSpeedPowerup)
            {
                moving *= speed * 2;
            }
            else
            {
                moving *= speed;
            }
            moving = transform.rotation * moving;

            myCharacterController.Move(moving * Time.deltaTime);
            dirtParticle.Play();

            if (transform.position.x < -48)
            {
                transform.position = new Vector3(-48, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 48)
            {
                transform.position = new Vector3(48, transform.position.y, transform.position.z);
            }
            if (transform.position.z < -7)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -7);
            }
            if (transform.position.z > 187)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 187);
            }
            if (transform.position.y < 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
            if (transform.position.y > 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
        }
    }

    void Rotation()
    {
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X")*4, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Food"))
        {
            newScore += 10;
            Destroy(other.gameObject);
            playerAudio.PlayOneShot(foodSound, 1.0f);
        }
        if (other.CompareTag("SpeedPowerup") && hasSpeedPowerup == false)
        {
            hasSpeedPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(SpeedPowerupCountdownRoutine());
            playerAudio.PlayOneShot(powerupSound, 1.0f);
        }
        if (other.CompareTag("DogPowerup") && hasDogPowerup == false)
        {
            hasDogPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(DogPowerupCountdownRoutine());
            SpawnManager.dogIndicatorIsActive = true;
            playerAudio.PlayOneShot(powerdownSound, 1.0f);
        }
    }

    IEnumerator SpeedPowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(10);
        hasSpeedPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    IEnumerator DogPowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(10);
        hasDogPowerup = false;
        SpawnManager.dogIndicatorIsActive = false;
    }
}