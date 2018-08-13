using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : CardLayout {

  public int size = 10;

  public override bool AddCard(Card c) {

    base.AddCard(c);

    return true;
  }
}
