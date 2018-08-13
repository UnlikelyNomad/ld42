using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WingStats {
  public int population;
  public int food;
  public int entertainment;
  public int reactor;
  public int size;
}

public enum AssignResult {
  Success,
  NoSpace,
  InsufficientResources,
  DuplicateModifier,
  IncompatibleSpecies
}

public class StationWing : CardLayout {

  public Text statText;

  public WingStats baseStats;
  public WingStats currentStats;

  public bool canAssign = false;

  public Selectable selectable;

  public Color assignedColor = Color.gray;
  public bool assigned = false;

  public Image panel;

  private void Start() {
    UpdateStats();
    panel.color = assignedColor;
  }

  public void UpdateStats() {
    currentStats.population = 0;
    currentStats.food = 0;
    currentStats.entertainment = 0;
    currentStats.reactor = 0;
    currentStats.size = baseStats.size;

    foreach (Card c in cards) {
      currentStats.population += c.info.population;
      currentStats.food += c.info.food;
      currentStats.entertainment += c.info.entertainment;
      currentStats.reactor += c.info.reactor;
      currentStats.size += c.info.size;
    }

    List<WingStats> modifiers = new List<WingStats>();
    foreach (Card c in cards) {
      if (c.info.isModifier) {
        modifiers.Add(c.info.modifier.ModifyOnStats(this, c));
      }
    }

    foreach (WingStats modifierStats in modifiers) {
      currentStats.population += modifierStats.population;
      currentStats.food += modifierStats.food;
      currentStats.entertainment += modifierStats.entertainment;
      currentStats.reactor += modifierStats.reactor;
      currentStats.size += modifierStats.size;
    }

    statText.text = "P:" + currentStats.population + " F:" + currentStats.food + " E:" + currentStats.entertainment + " R:" + currentStats.reactor + " S:" + AvailableSize();
  }

  public bool IsAssigned() {
    return assigned;
  }

  public void AssignColor(Color c) {
    assignedColor = c;
    Color a = c;
    a.a = 0.5f;
    panel.color = a;
    assigned = true;
  }

  public int AvailableSize() {
    return currentStats.size;
  }

  public AssignResult CanAssignCard(Card c) {
    if (AvailableSize() + c.info.size < 0) {
      return AssignResult.NoSpace;
    }

    int newFood = currentStats.food + c.info.food;
    int newEnt = currentStats.entertainment + c.info.entertainment;
    int newPow = currentStats.reactor + c.info.reactor;

    if (newFood < 0 || newEnt < 0 || newPow < 0) {
      return AssignResult.InsufficientResources;
    }

    if (c.info.isModifier && !c.IsPopulation()) {
      foreach (Card a in cards) {
        if (a.info.isModifier && a.info.cardName == c.info.cardName) {
          return AssignResult.DuplicateModifier;
        }
      }
    }

    if (assigned) {
      if (c.info.species == Species.UNSPECIFIED || c.info.species == Species.Robot) {
        return AssignResult.Success;
      }

      if (c.info.cardColor != assignedColor) {
        return AssignResult.IncompatibleSpecies;
      }
    }

    return AssignResult.Success;
  }
  public void EndRound() {

    UpdateStats();
  }

  public override bool CanSelectCard(Card c) {
    if (cards.Contains(c)) {
      return !c.IsPopulation();
    }

    return false;
  }

  public int Score() {
    return currentStats.population;
  }

  public override bool AddCard(Card c) {

    if (!assigned && c.info.species != Species.UNSPECIFIED && c.info.species != Species.Robot) {
      AssignColor(c.info.cardColor);
    }

    base.AddCard(c);
    UpdateStats();

    return true;
  }

  public override Card RemoveCard(Card c) {
    Card ret = base.RemoveCard(c);
    UpdateStats();
    CheckSpecies();
    return ret;
  }

  void CheckSpecies() {
    foreach (Card c in cards) {
      if (c.IsPopulation() && c.info.species != Species.Robot) {
        return;
      }
    }

    AssignColor(Color.gray);
    assigned = false;

  }

  public void SetCanAssign(bool canAssign) {
    selectable.interactable = canAssign;
  }
}
