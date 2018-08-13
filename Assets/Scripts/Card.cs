using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class CardAttributes {
  public string cardName;
  public string description;
  public Color sectionColor;
  public Color cardColor;

  public bool isModifier = false;

  public Species species;

  public Sprite art;

  public ModifierCard modifier = null;

  public bool Random = false;

  public int population;
  public int food;
  public int entertainment;
  public int reactor;
  public int size;
}

public enum Species {
  UNSPECIFIED,
  Human,
  Robot,
  Slug,
  Snake,
  Ork,
  Bee,
  Lizard,
  Bear
}

public enum CardType {
  Population,
  Food,
  Entertainment,
  Reactor,
  Modifier
}

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {

  public CardAttributes info;

  public Text title;
  public Image[] sections;
  public Text flavor;
  public Image outline;
  public Image artImage;

  public float scale = 0.5f;

  public void OnPointerClick(PointerEventData eventData) {
    //GetComponentInParent<CardLayout>().ToggleSelected(this);
    GameManager.Instance.ToggleSelected(this, GetComponentInParent<CardLayout>());
  }

  public void OnPointerEnter(PointerEventData eventData) {
    GetComponentInParent<CardLayout>().BringToFront(this);
  }

  public void SetInfo(CardAttributes ca) {
    info = ca;

    if (title != null) {
      title.text = info.cardName;
    }

    if (flavor != null) {
      flavor.text = info.description;
    }

    foreach (Image i in sections) {
      i.color = info.sectionColor;
    }

    if (outline != null) {
      outline.color = info.cardColor;
      outline.transform.localScale = new Vector3(scale, scale);
    }

    if (artImage != null) {
      artImage.sprite = info.art;
    }
  }

  public bool IsPopulation() {
    return info.population > 0;
  }

  public bool IsFood() {
    return info.food > 0;
  }

  public bool IsEntertainment() {
    return info.entertainment > 0;
  }

  public bool IsReactor() {
    return info.reactor > 0;
  }
}
