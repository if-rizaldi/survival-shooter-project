using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    [Header("Survival Control")]
    [SerializeField] private float normalWaveTime;
    [SerializeField] private float finalWaveTime;


    [Header("Enemy Controller")]
    private int maxEnemyCount;
    [SerializeField] private GameObject enemy;
    private int currentEnemyCount;
    [SerializeField] private float minDistanceFromPlayer;

    /*
        [Header("Magic Ball Controller")]
        [SerializeField] private int maxMagicBallCount;
        [SerializeField] private GameObject magicBall;
    */



    [Header("UI Controller")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameplayeUI;
    [SerializeField] private GameObject scoreUI;
    [SerializeField] private GameObject finalScoreUI;

    [SerializeField] private GameObject androidControlUI;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject dodgeButton;

    [SerializeField] private TextMeshProUGUI textGuider;
    [SerializeField] private TextMeshProUGUI countDown;
    [SerializeField] private TextMeshProUGUI fpsText;


    [SerializeField] private bool isAndroid;


    private ThirdPersonInputActionsAsset playerActionAsset;
    private GameObject player;
    private PlayerStats playerStats;


    private bool isPaused = false;
    private int playerScore;
    float currentTime;
    bool isPlayerAbleToDodge;


    void Awake()
    {
        Time.timeScale = 1;
        isPaused = false;
        gameplayeUI.SetActive(true);
        pauseMenu.SetActive(false);
        Application.targetFrameRate = 60;

        playerActionAsset = new ThirdPersonInputActionsAsset();
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        countDown = countDown.gameObject.GetComponent<TextMeshProUGUI>();
        textGuider = textGuider.gameObject.GetComponent<TextMeshProUGUI>();

        maxEnemyCount = playerData.SurvivalModeEnemyCount;
        playerScore = playerData.playerLastScore;
        isPlayerAbleToDodge = playerData.isPlayerAbleToDodge;
        isAndroid = playerData.isAndroidControl;

        finalScoreUI.GetComponent<Canvas>().enabled = false;
        scoreUI.GetComponent<Canvas>().enabled = false;

    }


    void Start()
    {
        //StartCoroutine(SpawnObjects(magicBall, maxMagicBallCount, 0.15f));
        StartCoroutine(GameStart());
        StartCoroutine(FramesPerSecond());


        //mengatur control android
        if (!isAndroid)
        {
            androidControlUI.GetComponent<Canvas>().enabled = false;
            pauseButton.transform.GetChild(0).GetComponent<Canvas>().enabled = false;
            pauseButton.transform.GetChild(1).gameObject.SetActive(true);
            Cursor.visible = true;

        }
        else
        {
            androidControlUI.GetComponent<Canvas>().enabled = true;
            pauseButton.transform.GetChild(0).GetComponent<Canvas>().enabled = true;
            pauseButton.transform.GetChild(1).gameObject.SetActive(false);
            Cursor.visible = true;

            if (!isPlayerAbleToDodge)
                dodgeButton.SetActive(false);
            else
                dodgeButton.SetActive(true);

        }
    }

    private IEnumerator FramesPerSecond()
    {
        while (true)
        {
            int fps = (int)(1f / Time.deltaTime);
            DisplayFPS(fps);

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void DisplayFPS(float fps)
    {
        fpsText.text = $"{fps} FPS";
    }


    void OnEnable()
    {
        playerActionAsset.Enable();
        playerActionAsset.UI.Pause.performed += DoPause;
    }

    void OnDisable()
    {
        playerActionAsset.Disable();
        playerActionAsset.UI.Pause.performed -= DoPause;
    }

    IEnumerator GameStart()
    {
        countDown.text = " ";
        textGuider.text = "Get Ready";
        yield return new WaitForSeconds(2f);

        textGuider.text = "3";
        yield return new WaitForSeconds(1f);

        textGuider.text = "2";
        yield return new WaitForSeconds(1f);

        textGuider.text = "1";
        yield return new WaitForSeconds(1f);

        textGuider.text = "Let's Rock!";
        yield return new WaitForSeconds(2f);

        //Gelombang Serangan Normal
        currentTime = normalWaveTime;
        textGuider.text = " ";
        countDown.text = "Time Left: \n" + currentTime.ToString();
        StartCoroutine(SpawnEnemyContinously(enemy, maxEnemyCount, .5f, currentTime));
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            currentTime--;
            countDown.text = "Time Left: \n" + currentTime.ToString();
        }

        //Gelombang Serangan Besar Pertama
        currentTime = finalWaveTime;
        textGuider.text = "First Wave";
        countDown.text = " ";
        yield return new WaitForSeconds(3f);
        StartCoroutine(SpawnEnemyContinously(enemy, 2 * maxEnemyCount, .1f, currentTime));
        textGuider.text = " ";
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            currentTime--;
            countDown.text = "First Wave";
        }
        yield return new WaitForSeconds(2f);


        //Kembali ke gelombang serangan normal
        currentTime = normalWaveTime;
        textGuider.text = " ";
        countDown.text = "Time Left: \n" + currentTime.ToString();
        StartCoroutine(SpawnEnemyContinously(enemy, maxEnemyCount, .5f, currentTime));
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            currentTime--;
            countDown.text = "Time Left: \n" + currentTime.ToString();
        }


        //Gelombang Serangan Akhir (Terbesar)
        currentTime = finalWaveTime;
        textGuider.text = "Final Wave";
        countDown.text = " ";
        yield return new WaitForSeconds(3f);
        StartCoroutine(SpawnEnemyContinously(enemy, 4 * maxEnemyCount, .1f, currentTime));
        textGuider.text = " ";
        countDown.text = "Final Wave";
        yield return new WaitForSeconds(currentTime);

        StartCoroutine(FinalStage());
    }


    public void GoToNextLevel()
    {
        playerData.playerLastScore = playerScore;
        playerData.playerInitialLife = playerStats.playerLifeRemaining;
        playerData.SurvivalModeEnemyCount += maxEnemyCount;
        Loader.Load(Loader.Scene.Loading);
        Loader.Load(Loader.Scene.PreparationMenu);
    }


    public void GoToMainMenu()
    {
        playerData.ScoreCalulation(playerScore);
        playerData.SurvivalModeReset();
        Loader.Load(Loader.Scene.Loading);
        Loader.Load(Loader.Scene.MainMenu);

    }

    //untuk fitur selanjutnya
    public void Collectible(Component sender, object data)
    {
        if (data is CollectibleType)
        {
            CollectibleType collectible = (CollectibleType)data;
            if (collectible == CollectibleType.MagicBall)
            {

                //pick the buff 
            }
        }
    }

    IEnumerator FinalStage()
    {
        while (true)
        {
            countDown.text = currentEnemyCount + " enemy left";
            if (currentEnemyCount <= 0)
            {
                Cursor.visible = true;
                playerData.ScoreCalulation(playerScore);
                gameplayeUI.SetActive(false);
                pauseButton.GetComponent<Canvas>().enabled = false;
                scoreUI.GetComponent<Canvas>().enabled = true;
                Time.timeScale = 0;
            }
            else
            {
                Debug.Log(currentEnemyCount);
            }
            yield return null;
        }

    }

    void PlayerGameOver()
    {
        Cursor.visible = true;
        playerData.ScoreCalulation(playerScore);
        gameplayeUI.SetActive(false);
        pauseButton.SetActive(false);
        finalScoreUI.GetComponent<Canvas>().enabled = true;
        Time.timeScale = 0;
    }




    //listen apakah player GameOver (menggunakan Game Event)
    public void OnPlayerGameOver(Component sender, object data)
    {
        if (data is bool)
        {
            bool gameover = (bool)data;
            if (gameover)
            {
                PlayerGameOver();
            }
        }
    }

    //listen jumlah score yang diperoleh player (menggunakan Game Event)
    public void OnPlayerScoreChanged(Component sender, object data)
    {
        if (data is int)
        {
            int score = (int)data;
            playerScore += score;
            playerData.playerScore = playerScore;
        }

    }

    //listen jumlah musuh yang berada di scene (menggunakan Game Event)
    public void OnEnemyCountChanged(Component sender, object data)
    {
        if (data is int)
        {
            int count = (int)data;
            currentEnemyCount += count;
        }

    }



    //panggil fungsi pause saat tombol pause ditekan
    private void DoPause(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            if (isPaused)
            {
                GamePause(false);
            }

            else
            {
                GamePause(true);
            }
        }
    }

    //mengatur pause pada game
    void GamePause(bool pauseState)
    {
        if (pauseState)
        {
            Cursor.visible = true;
            Time.timeScale = 0;
            isPaused = true;
            gameplayeUI.SetActive(false);
            pauseMenu.SetActive(true);
        }
        else
        {
            if (!isAndroid)
                Cursor.visible = true;

            Time.timeScale = 1;
            isPaused = false;
            gameplayeUI.SetActive(true);
            pauseMenu.SetActive(false);
        }
    }


    //Munuculkan musuh secara random di sepanjang navmesh dengan jarak tertentu dari player
    private IEnumerator SpawnEnemyContinously(GameObject spawnObject, int numberOfObject, float delayTime, float duration)
    {
        Collider[] hitColliders;
        float t = 0;
        while (t <= duration)
        {
            t += Time.deltaTime;
            if (currentEnemyCount < numberOfObject)
            {
                float walkRadius = minDistanceFromPlayer;

                Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
                randomDirection += player.transform.localPosition;

                Vector3 finalPosition = randomDirection;
                finalPosition.y = 1.5f;
                if (Vector3.Distance(player.transform.localPosition, finalPosition) < 20f)
                {

                    finalPosition.z = player.transform.localPosition.z + 20;
                    finalPosition.x = player.transform.localPosition.z + 20;
                }

                NavMeshHit hit;
                if (NavMesh.SamplePosition(finalPosition, out hit, walkRadius, 1))
                {

                    NavMesh.SamplePosition(finalPosition, out hit, walkRadius, 1);

                    finalPosition = hit.position;
                    finalPosition.y = 1.5f;

                    Vector3 boxSize = new Vector3(1f, 2f, 1f);
                    hitColliders = Physics.OverlapBox(finalPosition, boxSize, Quaternion.identity, LayerMask.GetMask("Obstacles")); // mencari semua collider dari objek static yang overlap dengan kotak di dodgeDirection
                    if (hitColliders.Length == 0)
                    {

                        Instantiate(spawnObject, finalPosition, Quaternion.identity);
                        yield return new WaitForSeconds(delayTime);
                        Debug.DrawRay(finalPosition, Vector3.up * 5, Color.green, 10f);


                    }
                    else
                    {

                        Debug.DrawRay(finalPosition, Vector3.up * 5, Color.red, 10f);

                    }

                }


            }

            yield return null;

        }



    }

    //Munuculkan objek secara random di sepanjang navmesh dengan jarak tertentu dari player (fitur buff)
    private IEnumerator SpawnObjects(GameObject spawnObject, int numberOfObject, float delayTime)
    {

        for (int i = 0; i <= numberOfObject; i++)
        {
            float walkRadius = minDistanceFromPlayer;


            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += player.transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1))
            {
                NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
                Vector3 finalPosition = hit.position;
                finalPosition.y = 1.5f;
                Instantiate(spawnObject, finalPosition, Quaternion.identity);
                yield return new WaitForSeconds(delayTime);

            }


        }
    }
}

