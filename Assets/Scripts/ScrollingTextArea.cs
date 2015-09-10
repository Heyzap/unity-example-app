using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollingTextArea : MonoBehaviour {

    public Vector2 scrollPosition;
    private string text = "";

    public int height;
    public int xPadding;
    public int yPadding;

    private Texture2D consoleBackgroundTexture;
    private GUIStyle consoleTextStyle;
    private GUIStyle consoleBackgroundStyle;
    void Start () {
        consoleBackgroundTexture = new Texture2D(1,1);
        for(var i = 0; i < consoleBackgroundTexture.width; i++)
        {
            for(var j = 0; j < consoleBackgroundTexture.height; j++)
            {
                consoleBackgroundTexture.SetPixel(i, j, new Color(0f, 0f, 0f, 0.7f));
            }
        }
        consoleTextStyle = new GUIStyle();
        consoleTextStyle.fontSize = 28;
        consoleTextStyle.normal.textColor = Color.white;
        consoleTextStyle.richText = true;
        consoleBackgroundStyle = new GUIStyle();
        consoleBackgroundStyle.normal.background = consoleBackgroundTexture;
    }

    void OnGUI() {
        GUI.skin.verticalScrollbar.fixedWidth = Screen.width * 0.05f;
        GUI.skin.verticalScrollbarThumb.fixedWidth = Screen.width * 0.05f;

        GUI.skin.horizontalScrollbar.fixedHeight = Screen.width * 0.035f;
        GUI.skin.horizontalScrollbarThumb.fixedHeight = Screen.width * 0.035f;


        GUI.BeginGroup(new Rect (xPadding, Screen.height - height - yPadding, Screen.width-xPadding*2, height), consoleBackgroundStyle);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false/*horizontal scroll bar*/, true/*vertical scroll bar*/, GUILayout.Width(Screen.width-xPadding*2), GUILayout.Height(height));
        GUILayout.Label(text, consoleTextStyle);
        GUILayout.EndScrollView();
        
        GUI.EndGroup();
    }

    public void Append(string append, bool newline=true) {
        if (newline && this.text.Length > 0)
            this.text += "\n";
        this.text += "[" + System.DateTime.UtcNow.ToString("HH:mm:ss") + "] ";
        this.text += append;
        this.scrollPosition = new Vector2(0, Mathf.Infinity);
    }
}
