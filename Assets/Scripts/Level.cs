using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "New Level")]
public class Level : ScriptableObject {

    public int ID;
    public Card[] cards;
    public float cardScale;
    public Vector2 gridStartLocation;

}
