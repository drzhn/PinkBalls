using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(ScriptsOrder.GameManager)]
public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameSettings settings;

    public event Action<int> OnScoreUpdated;
    public event Action<float> OnGameStarted;
    public event Action OnGameEnded;

    public static GameManager Instance => _instance;
    private static GameManager _instance;


    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            DestroyImmediate(this);
        }
    }

    void Start()
    {
        ObjectPooler.Instance.OnPoolReady += StartGame;
    }

    private void OnDisable()
    {
        ObjectPooler.Instance.OnPoolReady -= StartGame;
    }

    void StartGame()
    {
        Debug.Log("Game started");
        _score = 0;
        StartCoroutine(SpawningCoroutine());
    }

    private float timeRemain = 0;
    private int _score;
    private float aspectRatio;

    IEnumerator SpawningCoroutine()
    {
        timeRemain = settings.playTime;
        Time.timeScale = 1;
        OnGameStarted?.Invoke(timeRemain);
        float spawnTime = 0;
        while (timeRemain > 0)
        {
            timeRemain -= Time.deltaTime;
            spawnTime += Time.deltaTime;
            if (spawnTime > settings.ballCreatingFrequency)
            {
                spawnTime = 0;
                CreateBall();
            }

            yield return null;
        }

        Time.timeScale = Mathf.Epsilon;
        OnGameEnded?.Invoke();
    }

    void CreateBall()
    {
        float random = Random.value;
        float ballScaleFactor = settings.ballScaleMin + (settings.ballScaleMax - settings.ballScaleMin) * random;
        float speed = settings.speedOnMinScale + (settings.speedOnMaxScale - settings.speedOnMinScale) * random;
        int score = (int) (settings.scoreForMinBall + (settings.scoreForMaxBall - settings.scoreForMinBall) * random);

        Vector3 pos = new Vector3(
            Random.Range(-mainCamera.orthographicSize * mainCamera.aspect + ballScaleFactor, mainCamera.orthographicSize * mainCamera.aspect - ballScaleFactor),
            -ballScaleFactor,
            0);

        var ball = ObjectPooler.Instance.GetObject(PoolObject.Ball).GetComponent<Ball>();
        ball.transform.position = pos;
        ball.transform.localScale = Vector3.one * ballScaleFactor;
        ball.OnTap += UpdateScore;
        ball.Init(speed, settings.playTime, timeRemain, settings.nearTheEndSpeedMultiplier, mainCamera.orthographicSize * 2 + ballScaleFactor, score);
    }

    void UpdateScore(int score)
    {
        _score += score;
        OnScoreUpdated?.Invoke(_score);
    }

    public void StartAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

public abstract class ScriptsOrder
{
    public const int GameManager = Int32.MinValue + 0;
    public const int ObjectPooler = Int32.MinValue + 1;
    public const int Managers = Int32.MinValue + 2;
}