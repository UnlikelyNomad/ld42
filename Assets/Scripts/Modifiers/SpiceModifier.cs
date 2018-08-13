using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiceModifier : ModifierCard {

  public override WingStats ModifyOnStats(StationWing wing, Card self) {
    WingStats stats = new WingStats();

    int ent = 0;
    bool isFirst = false;
    foreach (Card c in wing.cards) {
      if (c.info.isModifier && c.info.cardName == self.info.cardName) {
        if (c == self) {
          isFirst = true;
        } else if (!isFirst) {
          Debug.Log("Found other spice card.");
          return stats;
        }
      } else if (c.IsEntertainment()) {
        ent += c.info.entertainment;
      }
    }

    stats.entertainment = ent;

    return stats;
  }
}
