using System;
using UnityEngine;

namespace DesignPatterns.Generics
{
    /// <summary>
    /// this class is a generic singleton implementation for Unity MonoBehaviour classes. 
    /// It ensures that only one instance of the specified type T exists in the scene at any given time. 
    /// If an instance does not already exist, it will attempt to find one in the scene or create a new one if necessary. 
    /// The singleton instance is accessible via the static Instance property.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();
        public static T Instance
        {
            get
            {
                lock (_lock) // lucchetto per assicurarci che ci sia un solo thread o coroutine che prova a chiamare questo Get
                {
                    if (_instance == null) // in questo caso è la prima volta che viene chiamato il get di Instance
                    {
                        _instance = FindFirstObjectByType<T>();

                        if (_instance == null)
                        {
                            try
                            {
                                var loadedObject = Resources.Load<GameObject>(typeof(T).Name); // il nome del GameObject prefab deve essere lo stesso del Tipo passato,
                                                                                               // ad esempio se voglio istanziare un LevelManager il prefab si chiamerà LevelManager.prefab

                                if (loadedObject != null) 
                                    Instantiate(loadedObject, Vector3.zero, Quaternion.identity);
                                else
                                {
                                    GameObject singletonObject = new GameObject(typeof(T).Name);
                                    _instance = singletonObject.AddComponent<T>();
                                    DontDestroyOnLoad(singletonObject);
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);
                            }
                        }

                    }

                    return _instance;
                }
            }
        }

        public virtual void Awake()
        {
            lock (_lock)
            {
                if (_instance != null && _instance != this)
                {
                    Destroy(gameObject);
                    return;
                }
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}


