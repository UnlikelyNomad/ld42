using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** event types:

  -wing starvation
  -wing riot
  -wing boredom
  -wing good

  -population shuttle
  -meteors
  -solar flare (power loss)
  -space scrap (upgrades)
  -space parasites (food loss)
  -disease
  -smugglers (cops)
*/

[System.Serializable]
public class EventInfo {
  public string title;
  public string message;
  public Sprite sprite;
  
  public Species species = Species.UNSPECIFIED;

  public List<int> specificCards = new List<int>();

  public int speciesCardIndex = -1;

  [Range(0, 3)]
  public int speciesCountRange = 1;

  [Range(0, 2)]
  public int randomCards = 3;
}

public class GameEvent {
  public List<Card> cards = new List<Card>();
  public EventInfo info;
}

public class EventDeck : MonoBehaviour {
  public List<EventInfo> events = new List<EventInfo>();

  public CardDeck deck;

  public GameEvent RandomEvent() {
    int i = Random.Range(0, events.Count);

    return FromInfo(events[i]);
  }

  GameEvent FromInfo(EventInfo info) {
    GameEvent g = new GameEvent();
    g.info = info;

    if (info.speciesCardIndex >= 0) {
      int n = Random.Range(1, info.speciesCountRange);

      for (int i = 0; i < n; ++i) {
        g.cards.Add(deck.GetCardByIndex(info.speciesCardIndex));
      }
    }

    for (int i = 0; i < info.specificCards.Count; ++i) {
      g.cards.Add(deck.GetCardByIndex(info.specificCards[i]));
    }

    for (int i = 0; i < info.randomCards; ++i) {
      g.cards.Add(deck.RandomNonSpeciesCard());
    }

    return g;
  }
}
