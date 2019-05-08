using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : PoolingMonoBehaviour, IIntractable
{
    public event Action<int> OnTap;

    public void Init(float speed, float playTime, float remainTime, float speedScaleFactor, float maxY, int score)
    {
        _speed = speed;
        _playTime = playTime;
        _remainTime = remainTime;
        _maxY = maxY;
        _speedScaleFactor = speedScaleFactor;
        _score = score;
        GetComponent<Renderer>().material.color = new Color(
            Random.value,
            Random.value,
            Random.value
        );
    }

    private float _speed = 5;
    private float _playTime = 5;
    private float _remainTime = 5;
    private float _maxY = 5;
    private float _speedScaleFactor = 5;
    private int _score;

    void Update()
    {
        transform.position = transform.position +
                             Vector3.up *
                             _speed *
                             (1 + (_speedScaleFactor - 1) * (1 - _remainTime / _playTime)) * //Скорость будет равномерно возрастать с уменьшением оставшегося времени
                             Time.deltaTime;
        _remainTime -= Time.deltaTime;
        if (transform.position.y > _maxY || _remainTime < 0)
            ReturnToPool();
    }

    public void Tap()
    {
        OnTap?.Invoke(_score);
        ReturnToPool();
    }

    public override void ReturnToPool()
    {
        ObjectPooler.Instance.ReturnObject(this);
    }
}