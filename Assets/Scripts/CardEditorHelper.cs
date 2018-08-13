using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CardEditorHelper : MonoBehaviour {

  Card c;

	// Use this for initialization
	void Start () {
    c = GetComponent<Card>();
	}
	
	// Update is called once per frame
	void Update () {
    if (c.title != null) {
      c.title.text = c.info.cardName;
    }

    if (c.flavor != null) {
      c.flavor.text = c.info.description;
    }

    foreach (Image i in c.sections) {
      i.color = c.info.sectionColor;
    }

    if (c.outline != null) {
      c.outline.color = c.info.cardColor;
      c.outline.transform.localScale = new Vector3(c.scale, c.scale);
    }

    if (c.artImage != null) {
      c.artImage.sprite = c.info.art;
    }
  }
}
