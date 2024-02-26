using UnityEngine;
using UnityEditor;
using System.IO;
using System.Reflection;

[CustomEditor(typeof(ReadMe))]
[InitializeOnLoad]
public class ReadMeEditor : Editor {
    [SerializeField] GUIStyle link;
    [SerializeField] GUIStyle title;
    [SerializeField] GUIStyle heading;
    [SerializeField] GUIStyle body;

    static string kShowedReadMeSessionStateName = "ReadmeEditor.showedReadme";
    static float kSpace = 16f;

    bool isInitialized;

    GUIStyle Link => link;
    GUIStyle Title => title;
    GUIStyle Heading => heading;
    GUIStyle Body => body;

    static ReadMeEditor() {
        EditorApplication.delayCall += AutoSelectReadMe;
    }

    static void AutoSelectReadMe() {
        if (!SessionState.GetBool(kShowedReadMeSessionStateName, false)) {
            var readMe = SelectReadMe();
            SessionState.SetBool(kShowedReadMeSessionStateName, true);

            if (readMe && !readMe.loadedLayout) {
                LoadLayout();
                readMe.loadedLayout = true;
            }
        }
    }

    static void LoadLayout() {
        var assembly = typeof(EditorApplication).Assembly;
        var windowLayoutType = assembly.GetType("UnityEditor.WindowLayout", true);
        var method = windowLayoutType.GetMethod("LoadWindowLayout", BindingFlags.Public | BindingFlags.Static);
        method.Invoke(null, new object[] { Path.Combine(Application.dataPath, "TutorialInfo/Layout.wlt"), false });
    }

    [MenuItem("Tutorial/Show Tutorial Instructions")]
    static ReadMe SelectReadMe() {
        var ids = AssetDatabase.FindAssets("ReadMe t:ReadMe"); //Unity isn't able to find this
        if (ids.Length == 1) {
            var readMe = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));
            Selection.objects = new UnityEngine.Object[] { readMe };
            return (ReadMe) readMe;
        } else {
            Debug.Log("Couldn't find a ReadMe");
            return null;
        }
    }

    protected override void OnHeaderGUI() {
        var readme = (ReadMe) target;
        Initalize();

        var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth / 3f - 20f, 128f);

        GUILayout.BeginHorizontal("In BigTitle");
        {
            GUILayout.Label(readme.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
            GUILayout.Label(readme.title, Title);
        }
        GUILayout.EndHorizontal();
    }

    public override void OnInspectorGUI() {
        var readMe = (ReadMe) target;
        Initalize();
        foreach (var section in readMe.sections) {
            if (!string.IsNullOrEmpty(section.heading))
                GUILayout.Label(section.heading, Heading);
            if (!string.IsNullOrEmpty(section.text))
                GUILayout.Label(section.text, Body);
            if (!string.IsNullOrEmpty(section.linkText)) {
                if (LinkLabel(new GUIContent(section.linkText)))
                    Application.OpenURL(section.url);
            }
            GUILayout.Space(kSpace);
        }
    }

    void Initalize() {
        if (!isInitialized) {
            body = new GUIStyle(EditorStyles.label);
            body.wordWrap = true;
            body.fontSize = 14;

            title = new GUIStyle(body);
            title.fontSize = 26;

            heading = new GUIStyle(body);
            heading.fontSize = 18;

            link = new GUIStyle(body);
            link.wordWrap = false;
            // Match selection color which works nicely for both light and dark skins
            link.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
            link.stretchWidth = false;

            isInitialized = true;
        }
    }

    bool LinkLabel(GUIContent label, params GUILayoutOption[] options) {
        var position = GUILayoutUtility.GetRect(label, Link, options);

        Handles.BeginGUI();
        Handles.color = Link.normal.textColor;
        Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
        Handles.color = Color.white;
        Handles.EndGUI();

        EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);
        return GUI.Button(position, label, Link);
    }
}
