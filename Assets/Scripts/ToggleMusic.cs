using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMusic : MonoBehaviour {

  public Sprite on;
  public Sprite off;

  public AudioSource source;

  public Image icon;

	// Use this for initialization
	void Start () {
	}

  void SetIcon() {
    icon.sprite = (source.mute ? off : on);
  }

  public void ToggleMute() {
    source.mute = !source.mute;
    SetIcon();
  }
	
	// Update is called once per frame
	void Update () {
		
	}
}
