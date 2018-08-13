using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngiModifier : ModifierCard {

  public override WingStats ModifyOnStats(StationWing wing, Card self) {
    WingStats stats = new WingStats();
    
    foreach (Card c in wing.cards) {
      if (c.info.isModifier && c.info.cardName == self.info.cardName) {
        if (c == self) {
          Debug.Log(c.gameObject.GetInstanceID() + ": Found self, setting size to 1");
          stats.size = 1;
          break;
        } else {
          Debug.Log(self.gameObject.GetInstanceID() + ": Found " + c.gameObject.GetInstanceID() + ", leaving");
          stats.size = 0;
          break;
        }
      }
    }

    return stats;
  }
}
