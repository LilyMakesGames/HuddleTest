using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo {

    public string Nome;
    public int Placar, Fase;
    
    public PlayerInfo(string name, int score, int level)
    {
        Nome = name;
        Placar = score;
        Fase = level;
    }
}
