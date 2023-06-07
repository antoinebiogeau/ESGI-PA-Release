using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]private GameLoop gameloop;

    public GameLoop Gameloop
    {
        get => gameloop;
        set => gameloop = value;
    }

    [SerializeField]private int turnCount;

    public int TurnCount
    {
        get => turnCount;
        set => turnCount = value;
    }

    [SerializeField]private int currentTurn;
    
    [SerializeField]private int score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
