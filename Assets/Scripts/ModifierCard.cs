using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierCard : MonoBehaviour {
  public virtual WingStats ModifyOnStats(StationWing wing, Card self) {
    WingStats stats = new WingStats();

    return stats;
  }
}
