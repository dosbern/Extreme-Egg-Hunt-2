using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] spawnList;
    public int spawnListIndex = 0;

    public GameObject[] carList;
    public int carListIndex = 0;

    public GameObject[] objectList;
    public int objectListIndex = 0;

    private float spawnRangeMaxX = 48;
    private float spawnRangeMinX = -48;
    private float spawnRangeMaxZ = 187;
    private float spawnRangeMinZ = -7;
    private float spawnTime = 1;
    private bool readyToSpawn = false;
    private bool readyToSpawnPowerup = false;

    public GameObject titleScreen;
    public GameObject dogPrefab;
    public GameObject speedPowerupPrefab;
    public GameObject dogPowerupPrefab;
    public GameObject dogIndicator;
    public static bool dogIndicatorIsActive = false;
    public Vector3 dogPosition;
    

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI dogScoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    public Button restartButton;

    public static bool isGameActive = true;
    public static int score;
    public static int dogScore;

    public static int gameDifficulty;

    private AudioSource buttonAudio;
    public AudioClip newGameSound;
    public AudioClip restartSound;

    // Start is called before the first frame update
    void Start()
    {
        buttonAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
            if (readyToSpawn && isGameActive == true)
            {
                spawnRandomFood();
                readyToSpawn = false;
                StartCoroutine(SpawnCountdownRoutine());
            }
            if (readyToSpawnPowerup && isGameActive == true)
            {
                spawnRandomPowerup();
                spawnRandomDogPowerup();
                readyToSpawnPowerup = false;
                StartCoroutine(PowerupCountdownRoutine());
            }
            UpdateScore();

        if (dogIndicatorIsActive == true)
        {
            dogPosition = GameObject.Find("Dog(Clone)").transform.position + new Vector3(0, 1, 0);
            dogIndicator.transform.position = dogPosition;
        }
        if (dogIndicatorIsActive == false)
        {
            dogIndicator.transform.position = new Vector3(0, -1, 0);
        }
    }

    void spawnRandomFood()
    {
        Vector3 spawnPos = new Vector3(Random.Range(spawnRangeMinX, spawnRangeMaxX), 0, Random.Range(spawnRangeMinZ, spawnRangeMaxZ));
        spawnListIndex = Random.Range(0, spawnList.Length);
        Instantiate(spawnList[spawnListIndex], spawnPos, spawnList[spawnListIndex].transform.rotation);
    }

    void spawnCars()
    {
        for(int i = 0; i < carList.Length; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-10, 10), 0, Random.Range(spawnRangeMinZ, spawnRangeMaxZ));
            carListIndex = i;
            Instantiate(carList[carListIndex], spawnPos, carList[carListIndex].transform.rotation);
        }
    }

    void spawnRandomObject()
    {
        Vector3 spawnPos = new Vector3(Random.Range(spawnRangeMinX, spawnRangeMaxX), 0, Random.Range(spawnRangeMinZ, spawnRangeMaxZ));
        objectListIndex = Random.Range(0, objectList.Length);
        Instantiate(objectList[objectListIndex], spawnPos, objectList[objectListIndex].transform.rotation);
    }

    IEnumerator SpawnCountdownRoutine()
    {
        yield return new WaitForSeconds(spawnTime);
        readyToSpawn = true;
    }

    void spawnRandomPowerup()
    {
        Vector3 spawnPos = new Vector3(Random.Range(spawnRangeMinX, spawnRangeMaxX), 0.6f, Random.Range(spawnRangeMinZ, spawnRangeMaxZ));
        Instantiate(speedPowerupPrefab, spawnPos, speedPowerupPrefab.transform.rotation);
    }

    void spawnRandomDogPowerup()
    {
        Vector3 spawnPos = new Vector3(Random.Range(spawnRangeMinX, spawnRangeMaxX), 0.43f, Random.Range(spawnRangeMinZ, spawnRangeMaxZ));
        Instantiate(dogPowerupPrefab, spawnPos, dogPowerupPrefab.transform.rotation);
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(5);
        readyToSpawnPowerup = true;
    }

    private void UpdateScore()
    {
        score = PlayerController.newScore;
        dogScore = DogController.newDogScore;
        scoreText.text = "Your Score: " + score;
        dogScoreText.text = "Enemy Score: " + dogScore;
        if (score >= 1000 || dogScore >= 1000)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;

        score = PlayerController.newScore;
        dogScore = DogController.newDogScore;
        if (score > dogScore)
        {
            winText.gameObject.SetActive(true);
        }
        else
        {
            loseText.gameObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        PlayerController.newScore = 0;
        DogController.newDogScore = 0;
        score = 0;
        dogScore = 0;
        UpdateScore();
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty)
    {
        buttonAudio.PlayOneShot(newGameSound, 1.0f);
        gameDifficulty = difficulty;

        spawnCars();

        for (int i = 0; i < 10; i++)
        {
            spawnRandomObject();
        }

        dogPrefab = (GameObject)Instantiate(dogPrefab, new Vector3(0, 0, 10), Quaternion.identity);
        dogIndicator = (GameObject)Instantiate(dogIndicator, new Vector3(0, 0, 10), Quaternion.identity);

        for (int i = 0; i < 100; i++)
        {
            spawnRandomFood();
        }

        StartCoroutine(SpawnCountdownRoutine());
        StartCoroutine(PowerupCountdownRoutine());

        score = 0;
        dogScore = 0;
        UpdateScore();
        isGameActive = true;
        titleScreen.gameObject.SetActive(false);
    }
}