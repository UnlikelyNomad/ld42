using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPillsModifier : ModifierCard {

  public override WingStats ModifyOnStats(StationWing wing, Card self) {
    WingStats stats = new WingStats();

    int foodReq = 0;
    foreach (Card c in wing.cards) {
      if (c.info.food < 0) {
        foodReq -= c.info.food;
      }
    }

    stats.food += foodReq / 2;
    return stats;
  }
}
