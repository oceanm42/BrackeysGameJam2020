using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    private Vector3 startPos;
    [SerializeField]
    private float minClamp;
    [SerializeField]
    private float maxClamp;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        startPos = transform.position;
    }

    private void Update()
    {
        if(gameManager.gameHasEnded == false)
        {
            transform.position = new Vector3(Mathf.Clamp(target.position.x, minClamp, maxClamp), startPos.y, startPos.z);
        }
    }
}
