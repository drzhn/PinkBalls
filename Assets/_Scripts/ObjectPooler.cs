using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Реализовал простенький пул объектов. Игра начнется тогда, когда созданы все нужные объекты
/// </summary>
[DefaultExecutionOrder(ScriptsOrder.ObjectPooler)]
public class ObjectPooler : MonoBehaviour
{
    public List<PoolEntity> entities;
    public Transform objectsParentTransform;
    // чтобы не фризить игру на старте, указываю, сколько instantiate можно за кадр пускать
    public int maxInitializationsPerFrame = 10;

    public event Action OnPoolReady;

    private readonly Dictionary<PoolObject, ObjectStorage> _freeEntities = new Dictionary<PoolObject, ObjectStorage>((int) PoolObject.Count);

    public static ObjectPooler Instance => _instance;
    private static ObjectPooler _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else 
        {
            DestroyImmediate(this);
        }
        objectsParentTransform.gameObject.SetActive(false);
    }

    private int _count = 0;

    IEnumerator Start()
    {
        _count = 0;
        foreach (PoolEntity entity in entities)
        {
            ObjectStorage data = new ObjectStorage()
            {
                InitCapacity = entity.amount,
                Entities = new Queue<GameObject>(entity.amount)
            };
            for (int i = 0; i < entity.amount; i++)
            {
                var obj = Instantiate(entity.prefab, objectsParentTransform, true);
                data.Entities.Enqueue(obj);
                _count++;
                if (_count >= maxInitializationsPerFrame) 
                {
                    _count = 0;
                    yield return null;
                }
            }

            _freeEntities.Add(entity.type, data);
        }

        Debug.Log("Pool is ready");
        OnPoolReady?.Invoke();
    }

    void Update()
    {
    }

    public GameObject GetObject(PoolObject type)
    {
        if (_freeEntities[type].Entities.Count <= _freeEntities[type].InitCapacity * 0.38f) // золотое сечение :D
        {
            var prefab = entities.Find(x => x.type == type).prefab;
            for (int i = 0; i < maxInitializationsPerFrame; i++)
            {
                _freeEntities[type].Entities.Enqueue(Instantiate(prefab, objectsParentTransform, true));
            }
        }

        GameObject o = _freeEntities[type].Entities.Dequeue();
        o.transform.SetParent(null);
        return o;
    }

    public void ReturnObject(PoolingMonoBehaviour o)
    {
        o.transform.SetParent(objectsParentTransform);
        o.transform.localPosition = Vector3.zero;

        _freeEntities[o.type].Entities.Enqueue(o.gameObject);
    }



    class ObjectStorage
    {
        public int InitCapacity = 0;
        public Queue<GameObject> Entities;
    }
}

[System.Serializable]
public class PoolEntity
{
    public PoolObject type;
    public GameObject prefab;
    public int amount;
}

public enum PoolObject
{
    Ball,
    Count
}

public abstract class PoolingMonoBehaviour : MonoBehaviour
{
    public PoolObject type;
    public abstract void ReturnToPool();
}