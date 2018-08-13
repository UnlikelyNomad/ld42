using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WingAssignHandler : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {

  public void OnPointerClick(PointerEventData eventData) {
    GameManager.Instance.AssignCards(GetComponentInParent<StationWing>());
  }

  public void OnPointerEnter(PointerEventData eventData) {
    
  }

  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
