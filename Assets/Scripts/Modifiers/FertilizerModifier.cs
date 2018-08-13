using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FertilizerModifier : ModifierCard {

  public override WingStats ModifyOnStats(StationWing wing, Card self) {
    WingStats stats = new WingStats();

    foreach (Card c in wing.cards) {
      if (c.IsFood()) {
        stats.food += c.info.food;
      }
    }
    return stats;
  }
}
