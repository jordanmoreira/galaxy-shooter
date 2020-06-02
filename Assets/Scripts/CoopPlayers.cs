using UnityEngine;

public class CoopPlayers : MonoBehaviour
{
    [SerializeField]
    public Player _playerOne;
    [SerializeField]
    public Player _playerTwo;

    // Start is called before the first frame update
    void Start()
    {
        _playerOne = GameObject.Find("Player_One").GetComponent<Player>();
        _playerTwo = GameObject.Find("Player_Two").GetComponent<Player>();
    }
}