using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventPanel : MonoBehaviour {

  public Text titleText;
  public Text messageText;
  public Image image;

  public CardLayout layout;

  private void Start() {
  }

  public void ShowEvent(string title, string message, Sprite sprite, List<Card> cards) {
    titleText.text = title;
    messageText.text = message;
    image.sprite = sprite;

    gameObject.SetActive(true);

    layout.ClearCards();

    for (int i = 0; i < cards.Count; ++i) {
      layout.AddCard(cards[i]);
    }
  }

  public void ShowEvent(EventInfo info, List<Card> cards) {
    ShowEvent(info.title, info.message, info.sprite, cards);
  }

  public void CloseEvent() {

    gameObject.SetActive(false);

    GameManager.Instance.FinishEvent();
  }
}
