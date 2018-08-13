using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SnapTo {
  TopLeft,
  TopRight,
  BottomLeft,
  BottomRight
}

public class CardLayout : MonoBehaviour {

  public Vector2 start;
  public Vector2 end;

  public SnapTo startSnap;
  public SnapTo endSnap;

  public RectTransform cardTransform;

  public bool verticalLayout = false;
  public bool canSelect = false;

  public float selectedOffset = 10f;

  public List<Card> cards = new List<Card>();
  public List<Card> selected = new List<Card>();

  public int selectLimit = 1;

  float cardWidth, cardHeight;

  RectTransform rectTransform;

	// Use this for initialization
	void Awake () {
    cardWidth = cardTransform.rect.width;
    cardHeight = cardTransform.rect.height;

    //if (rectTransform == null) {
      rectTransform = GetComponent<RectTransform>();
    //}
	}

  Vector2 RelativePosition(Vector2 pos, SnapTo corner) {
    Vector2 r = new Vector2();

    switch (corner) {
    case SnapTo.BottomLeft:
    case SnapTo.TopLeft:
      r.x = pos.x;
      break;

    case SnapTo.TopRight:
    case SnapTo.BottomRight:
      r.x = rectTransform.rect.size.x - pos.x;
      break;
    }

    switch (corner) {
    case SnapTo.TopLeft:
    case SnapTo.TopRight:
      r.y = rectTransform.rect.size.y - pos.y;
      break;

    case SnapTo.BottomLeft:
    case SnapTo.BottomRight:
      r.y = pos.y;
      break;
    }

    return r;
  }

  public void DoLayout() {
    if (!gameObject.activeInHierarchy) {
      return;
    }

    float span, size;
    
    Vector2 pxEnd = RelativePosition(end, endSnap);
    Vector2 pxStart = RelativePosition(start, startSnap);

    if (verticalLayout) {
      size = cardHeight;
      span = pxEnd.y - pxStart.y;
    } else {
      size = cardWidth;
      span = pxEnd.x - pxStart.x;
    }
      
    float r = Mathf.Abs(size / span);
    float rt = r * (cards.Count - 1);
    float s;
    float f;

    if (rt > 1f) {
      s = 0f;
      f = 1f / (cards.Count - 1);
    } else {
      s = 0.5f - (rt / 2f);
      f = r;
    }

    for (int i = 0; i < cards.Count; ++i) {
      RectTransform t = cards[i].GetComponent<RectTransform>();
      t.SetParent(rectTransform);
      float p = s + (i * f);
      t.anchoredPosition = Vector2.Lerp(pxStart, pxEnd, p);

      if (selected.Contains(cards[i])) {
        if (!verticalLayout) {
          t.anchoredPosition = new Vector2(t.anchoredPosition.x, t.anchoredPosition.y + selectedOffset);
        } else {
          t.anchoredPosition = new Vector2(t.anchoredPosition.x + selectedOffset, t.anchoredPosition.y);
        }
      }
    }
  }

  public virtual bool AddCard(Card c) {
    cards.Add(c);
    DoLayout();
    BringToFront(c);

    return true;
  }

  public virtual void ClearCards() {
    cards.Clear();
    DoLayout();
  }

  public void SelectCard(Card c) {
    if (!canSelect) {
      return;
    }

    if (selected.Count < selectLimit || selectLimit < 0) {
      selected.Add(c);
      DoLayout();
    }
  }

  public void DeselectCard(Card c) {
    selected.Remove(c);
  }

  public virtual Card RemoveCard(Card c) {
    Card ret = null;

    if (selected.Contains(c)) {
      selected.Remove(c);
    }

    if (cards.Contains(c)) {
      cards.Remove(c);

      ret = c;
    }

    DoLayout();

    return ret;
  }

  public virtual bool CanSelectCard(Card c) {
    return true;
  }

  public void ToggleSelected(Card c) {
    if (!canSelect) {
      return;
    }

    if (CanSelectCard(c)) {
      if (selected.Contains(c)) {
        selected.Remove(c);
      } else {
        if (cards.Contains(c)) {
          if (selectLimit < 0 || selected.Count < selectLimit) {
            selected.Add(c);
          }
        }
      }
    }

    DoLayout();
  }

  public void BringToFront(Card c) {
    int idx = cards.IndexOf(c);

    if (idx < 0) {
      return;
    }

    for (int i = 0; i < idx; ++i) {
      Reparent(cards[i]);
    }
    for (int i = cards.Count - 1; i >= idx; --i) {
      Reparent(cards[i]);
    }
  }

  void Reparent(Card c) {
    Transform t = c.transform;
    t.SetParent(null);
    t.SetParent(rectTransform);
  }
}
