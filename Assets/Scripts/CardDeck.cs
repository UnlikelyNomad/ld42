using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour {

  static CardDeck _instance = null;

  public CardDeck Instance {
    get {
      return _instance;
    }
  }

  public GameObject cardPrefab;

  public List<CardAttributes> cards = new List<CardAttributes>();

  void Awake() {
    if (_instance == null) {
      _instance = this;
    } else if (_instance != this) {
      Destroy(this);
    }
  }

  void Start() {
  }

  Card CardFromAttributes(CardAttributes ca) {
    GameObject g = (GameObject)Instantiate(cardPrefab);
    Card c = g.GetComponent<Card>();
    c.SetInfo(ca);
    return c;
  }

  public Card GetSpeciesCard(Species sp) {
    List<int> choice = new List<int>();
    for (int i = 0; i < cards.Count; ++i) {
      choice.Add(i);
    }

    while (choice.Count > 0) {
      int c = Random.Range(0, choice.Count);
      CardAttributes ca = cards[choice[c]];

      if (ca.species == sp) {
        return CardFromAttributes(ca);
      }

      choice.RemoveAt(c);
    }

    return null;
  }

  public Card RandomNonSpeciesCard() {
    List<int> choice = new List<int>();
    for (int i = 0; i < cards.Count; ++i) {
      choice.Add(i);
    }

    while (choice.Count > 0) {
      int c = Random.Range(0, choice.Count);
      CardAttributes ca = cards[choice[c]];

      if (ca.species == Species.UNSPECIFIED && ca.Random) {
        return CardFromAttributes(ca);
      }

      choice.RemoveAt(c);
    }

    return null;
  }

  public Card GetCardByIndex(int idx) {
    CardAttributes ca = cards[idx];
    return CardFromAttributes(ca);
  }
}
