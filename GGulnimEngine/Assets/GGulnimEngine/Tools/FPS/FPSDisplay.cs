using UnityEngine;

/// <summary>
/// [Project Setting] - [Other Setting] - [Scripting Define Symbols] - [Add] - [SHOW_FPS]
/// </summary>
public class FPSDisplay : MonoBehaviour
{
	public const string SHOW_FPS = "SHOW_FPS";

	public enum DISPLAY_POS_H { LEFT, RIGHT };
	public enum DISPLAY_POS_V { TOP, BOTTOM };

	private float _deltaTime = 0.0f;

	public DISPLAY_POS_H DispPosH = DISPLAY_POS_H.LEFT;
	public DISPLAY_POS_V DispPosV = DISPLAY_POS_V.TOP;

	private void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
    }

	private void OnGUI()
    {
		ShowFps();
	}

	[System.Diagnostics.Conditional(SHOW_FPS)]
	private void ShowFps()
	{
		int fontSize = Screen.height * 2 / 100;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0.0f, 0.0f, Screen.width, Screen.height);

		if (DispPosH == DISPLAY_POS_H.LEFT && DispPosV == DISPLAY_POS_V.TOP)
			style.alignment = TextAnchor.UpperLeft;
		else if (DispPosH == DISPLAY_POS_H.LEFT && DispPosV == DISPLAY_POS_V.BOTTOM)
			style.alignment = TextAnchor.LowerLeft;
		else if (DispPosH == DISPLAY_POS_H.RIGHT && DispPosV == DISPLAY_POS_V.TOP)
			style.alignment = TextAnchor.UpperRight;
		else if (DispPosH == DISPLAY_POS_H.RIGHT && DispPosV == DISPLAY_POS_V.BOTTOM)
			style.alignment = TextAnchor.LowerRight;

		style.fontSize = fontSize;
		style.normal.textColor = Color.white;
		float ms = _deltaTime * 1000.0f;
		float fps = 1.0f / _deltaTime;
		string text = $"{fps:0.} fps ({ms:0.0} ms)";
		GUI.Label(rect, text, style);
	}
}

