using System;
using UnityEngine;

public class ReadMe : ScriptableObject {
	public Section[] sections;
	public Texture2D icon;
	public string title;
	public bool loadedLayout;
	
	[Serializable]
	public class Section {
		public string heading, text, linkText, url;
	}
}
