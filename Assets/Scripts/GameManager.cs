using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour {

    System.Random rng = new System.Random();

    [SerializeField]
    GameObject cardObjectPrefab;
    Card[] cards;

    public bool canClick;

    [SerializeField]
    Level[] level;
    int currentlevel = 0;

    List<GameObject> cardsInGame = new List<GameObject>();
    GameObject cardRevealed1, cardRevealed2;

    public int score;
    public int[] scores;
    public Text scoreText;
    public GameObject panel,panel2;

    public InputField inputName;
    new string name;

    string json;

    void Start () {
        level = Resources.LoadAll<Level>("Levels");
        scores = new int[level.Length];
        canClick = true;

    }

    public void StartGame()
    {
        scoreText.gameObject.SetActive(true);
        name = inputName.text;
        panel.SetActive(false);
        InitializeLevel(currentlevel);
    }

    void InitializeLevel(int loadLevel)
    {
        cards = level[loadLevel].cards;
        for (int i = 0; i < cards.Length; i++)
        {
            GameObject card = Instantiate(cardObjectPrefab);
            card.GetComponent<CardObject>().card = cards[i];
            cardsInGame.Add(card);
            card.transform.localScale *= level[currentlevel].cardScale;
        }
        for (int i = 0; i < cards.Length; i++)
        {
            GameObject card = Instantiate(cardObjectPrefab);
            card.GetComponent<CardObject>().card = cards[i];
            cardsInGame.Add(card);
            card.transform.localScale *= level[currentlevel].cardScale;
        }

        Shuffle();
        OrganizeSquare();
    }
    
    void OrganizeSquare()
    {
        float sqrt = Mathf.Sqrt(cardsInGame.Count);
        int columns = Mathf.FloorToInt(sqrt);
        int lines = Mathf.CeilToInt(sqrt);
        if (cardsInGame.Count != columns * lines)
            lines++;
        for (int x = 0; x < lines; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                if(cardsInGame.Count > lines * y + x)
                {
                    cardsInGame[lines * y + x].transform.position = new Vector2(x * level[currentlevel].cardScale + level[currentlevel].gridStartLocation.x,-y * level[currentlevel].cardScale + level[currentlevel].gridStartLocation.y);
                    cardsInGame[lines * y + x].name = x.ToString() + "," + y.ToString();
                }
            }
        }
    }

    public void AddToList(GameObject cardRevealed)
    {
        if(cardRevealed1 == null)
        {
            cardRevealed1 = cardRevealed;
        }
        else if(cardRevealed2 == null)
        {
            cardRevealed2 = cardRevealed;
            canClick = false;
            StartCoroutine(WaitForReveal());

        }

    }

    void CompareCards()
    {

        if (cardRevealed1.GetComponent<CardObject>().ID == cardRevealed2.GetComponent<CardObject>().ID)
        {
            cardsInGame.Remove(cardRevealed1);
            cardsInGame.Remove(cardRevealed2);
            Destroy(cardRevealed1);
            Destroy(cardRevealed2);
            score += 100;
        }
        else
        {
            cardRevealed1.GetComponent<CardObject>().revealed = false;
            cardRevealed2.GetComponent<CardObject>().revealed = false;
            score -= 50;
            cardRevealed1 = null;
            cardRevealed2 = null;
        }
        scoreText.text = "SCORE: " + score.ToString();
        if (cardsInGame.Count == 0)
        {
            scores[currentlevel] = score;
            StartCoroutine(SendInformation());
            score = 0;
            currentlevel++;
            if (level.Length > currentlevel)
            {
                scoreText.text = "SCORE: " + score.ToString();
                InitializeLevel(currentlevel);
            }
            else
                EndGame();
        }

    }

    public void EndGame()
    {
        for (int s = 0; s < scores.Length; s++)
        {
            score += scores[s];
        }
        scoreText.text = "SCORE TOTAL: " + score.ToString();
        panel2.SetActive(true);
    }

    IEnumerator SendInformation()
    {
        PlayerInfo player = new PlayerInfo(name, score, (currentlevel+1));
        json = JsonUtility.ToJson(player);
        UnityWebRequest request = UnityWebRequest.Post("https://us-central1-huddle-team.cloudfunctions.net/api/memory/yuribarbosa.contato@gmail.com", json);
        yield return request.SendWebRequest();

        if(request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("Erro no envio!");
        }
        else
        {

        }
        print(json);
    }

    public void Shuffle()
    {
        int n = cardsInGame.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject value = cardsInGame[k];
            cardsInGame[k] = cardsInGame[n];
            cardsInGame[n] = value;
        }
    }

    IEnumerator WaitForReveal()
    { 
        yield return new WaitForSeconds(0.5f);
        CompareCards();
        canClick = true;
    }
}
