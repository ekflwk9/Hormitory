using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerController playerController;
    private Camera camera;

    public PlayerController PlayerController
    {
        get
        {
            if(playerController == null)
                playerController = FindObjectOfType<PlayerController>();
            return playerController;
        }
    }

    public Camera Camera
    {
        get
        {
            if (camera == null)
                camera = Camera.main;
            return camera;
        }
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
