using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompactSpaceModifier : ModifierCard {

  public override WingStats ModifyOnStats(StationWing wing, Card self) {
    WingStats stats = new WingStats();

    int pop = 0;
    foreach (Card c in wing.cards) {
      if (c.IsPopulation() && c.info.size < 0) {
        pop += c.info.population;
      }
    }

    stats.size = pop / 2;

    return stats;
  }
}
