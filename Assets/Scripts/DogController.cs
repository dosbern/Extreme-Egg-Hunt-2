using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    private float speed = 20f;
    private Rigidbody dogRb;
    private GameObject food;
    private Vector3 foodPos;
    private Vector3 tempVect;
    private Vector3 lookDirection;

    public static int newDogScore;

    private float spawnRangeMaxX = 48;
    private float spawnRangeMinX = -48;
    private float spawnRangeMaxZ = 187;
    private float spawnRangeMinZ = -7;

    private AudioSource dogAudio;
    public AudioClip enemyPickup;

    // Start is called before the first frame update
    void Start()
    {
        dogRb = GetComponent<Rigidbody>();
        dogAudio = GetComponent<AudioSource>();
        food = GameObject.FindGameObjectWithTag("Food");
        Rotation();
        newDogScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -48)
        {
            Rotation();
        }
        if (transform.position.x > 48)
        {
            Rotation();
        }
        if (transform.position.z < -7)
        {
            Rotation();
        }
        if (transform.position.z > 187)
        {
            Rotation();
        }

        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void Rotation()
    {
        tempVect = new Vector3(Random.Range(spawnRangeMinX, spawnRangeMaxX), 0, Random.Range(spawnRangeMinZ, spawnRangeMaxZ));
        transform.LookAt(tempVect);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food") && SpawnManager.isGameActive == true && !SpawnManager.dogIndicatorIsActive)
        {
            newDogScore += (SpawnManager.gameDifficulty);
            Destroy(other.gameObject);
            Rotation();
            dogAudio.PlayOneShot(enemyPickup, 1.0f);
        }
        if (other.CompareTag("Car") || other.CompareTag("Obstacle") || other.CompareTag("Player"))
        {
            Rotation();
        }
    }
}