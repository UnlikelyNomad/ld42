using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverdriveModifier : ModifierCard {

  public override WingStats ModifyOnStats(StationWing wing, Card self) {
    WingStats stats = new WingStats();

    foreach (Card c in wing.cards) {
      if (c.IsReactor()) {
        stats.reactor += c.info.reactor;
      }
    }

    return stats;
  }
}
