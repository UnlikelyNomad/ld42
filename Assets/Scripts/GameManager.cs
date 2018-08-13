using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState {
  INIT,
  EVENT,
  PLAY,
  DISCARD,
  END
}

public class GameManager : MonoBehaviour {

  static GameManager _instance = null;

  public static GameManager Instance {
    get { return _instance; }
  }

  public Hand hand;

  public List<StationWing> wings;

  public CardDeck deck;
  public EventDeck eventDeck;

  public EventPanel eventPanel;

  public Sprite eventSprite;

  List<Card> eventCards = new List<Card>();

  public Button endTurn;
  public Button discard;

  public GameState state = GameState.INIT;

  public Text scoreText;
  public Text discardedText;

  Vector2 res;

  public Text status;

  public List<int> startWingCards;

  public int maxHandSize = 10;

  public Text nextStepText;

  public GameObject endGamePanel;
  public Text endGameText;

  public int discardedPop = 0;

  public int score = 0;

  public Card selectedCard = null;
  public CardLayout selectedLayout = null;
  public bool hasSelected = false;

  private void Awake() {
    if (_instance == null) {
      _instance = this;
    } else if (_instance != this) {
      Destroy(this);
    }
  }

  // Use this for initialization
  void Start () {
    StartCoroutine(StartGame());
	}

  IEnumerator StartGame() {
    yield return null;

    InitGame();
  }

  bool CanMove() {
    foreach (Card c in hand.cards) {
      foreach (StationWing wing in wings) {
        if (wing.CanAssignCard(c) == AssignResult.Success) {
          return true;
        }
      }
    }

    return false;
  }

  public void FinishEvent() {
    //add cards to hand
    for (int i = 0; i < eventCards.Count; ++i) {
      hand.AddCard(eventCards[i]);
    }

    eventCards.Clear();

    if (!CanMove()) {
      EndGame("You have run out of moves!");
    }

    StartPlay();
  }

  void InitGame() {
    state = GameState.INIT;

    for (int i = 0; i < startWingCards.Count; ++i) {
      Card c = deck.GetCardByIndex(startWingCards[i]);
      hand.AddCard(c);
    }

    UpdateScore();

    StartEvent();
  }

  void StartEvent() {
    state = GameState.EVENT;
    status.text = "A New Event!";

    endTurn.interactable = false;

    hand.canSelect = false;
    hand.selectLimit = 0;

    GameEvent g = eventDeck.RandomEvent();
    eventCards = g.cards;

    eventPanel.ShowEvent(g.info, g.cards);
  }

  void StartPlay() {
    state = GameState.PLAY;
    status.text = "Play Cards to Your Station";

    nextStepText.text = "End Turn";

    hand.canSelect = true;
    hand.selectLimit = 1;
    foreach (StationWing wing in wings) {
      wing.SetCanAssign(true);
      wing.canSelect = true;
      wing.selectLimit = 1;
    }

    endTurn.interactable = true;
    discard.interactable = true;
  }

  public void ToggleSelected(Card c, CardLayout layout) {
    if (state == GameState.PLAY) {
      if (hasSelected && selectedCard != c) {
        selectedLayout.DeselectCard(selectedCard);
      }

      selectedLayout = layout;
      selectedCard = c;
      hasSelected = true;
    }

    layout.ToggleSelected(c);
  }

  void StartDiscard() {
    state = GameState.DISCARD;
    status.text = "Discard Extra Cards";

    nextStepText.text = "Discard";

    hand.selectLimit = -1;

    foreach (StationWing wing in wings) {
      wing.SetCanAssign(false);
    }

    List<Card> handSpecies = new List<Card>();

    foreach (Card c in hand.cards) {
      if (c.IsPopulation()) {
        handSpecies.Add(c);
      }
    }

    foreach (Card c in handSpecies) {
      hand.RemoveCard(c);
      Destroy(c.gameObject);
      discardedPop++;
    }

    if (hand.cards.Count <= maxHandSize) {
      EndRound();
    }

    if (discardedPop > score) {
      EndGame("The abandoned populations have overrun and destroyed your station in anger!");
    }
  }

  public void EndTurn() {
    if (state == GameState.PLAY) {
      StartDiscard();
    } else if (state == GameState.DISCARD) {

      while(hand.selected.Count > 0) {
        Card c = hand.RemoveCard(hand.selected[0]);
        Destroy(c.gameObject);
      }

      if (hand.cards.Count > maxHandSize) {
        return;
      }

      EndRound();
    }
  }

  void EndGame(string message) {
    hand.canSelect = false;
    foreach(StationWing wing in wings) {
      wing.canSelect = false;
    }
    endTurn.interactable = false;

    endGamePanel.SetActive(true);
    endGameText.text = message + "\nTry again or press Escape to quit.";
  }

  void EndRound() {
    //adjust scores
    foreach(StationWing wing in wings) {
      wing.EndRound();
    }

    UpdateScore();

    StartEvent();
  }

  void UpdateScore() {
    score = 0;

    foreach(StationWing wing in wings) {
      score += wing.Score();
    }

    scoreText.text = "Score: " + score;
    discardedText.text = "Discarded Pop: " + discardedPop;
  }

  public void Discard() {
    foreach (Card c in hand.selected) {
      DiscardHelper(c, hand);
    }

    foreach (StationWing wing in wings) {
      foreach (Card c in wing.selected) {
        DiscardHelper(c, wing);
      }
    }
  }

  void DiscardHelper(Card c, CardLayout layout) {
    if (c.IsPopulation()) {
      discardedPop++;
    }

    layout.RemoveCard(c);
    Destroy(c.gameObject);
  }

  public void AssignCards(StationWing wing) {
    int idx = 0;
    while (hand.selected.Count > idx) {
      Card c = hand.selected[idx];

      AssignResult res = wing.CanAssignCard(c);
      Debug.Log(res);
      if (res == AssignResult.Success) {
        wing.AddCard(hand.RemoveCard(c));
      } else {
        ++idx;
      }
    }
  }

  public void RestartGame() {
    SceneManager.LoadScene(0);
  }

  IEnumerator HandleResize() {
    yield return null;

    hand.DoLayout();
    foreach (StationWing wing in wings) {
      wing.DoLayout();
    }
    eventPanel.layout.DoLayout();
  }

  private void Update() {
    if (res.x != Screen.width || res.y != Screen.height) {
      StartCoroutine(HandleResize());
    }

    res.x = Screen.width;
    res.y = Screen.height;

    if (Input.GetKeyDown(KeyCode.Escape)) {
      Application.Quit();
    }
  }
}
