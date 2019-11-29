using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ToneGenerator))]
public class ToneGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ToneGenerator toneGenerator = (ToneGenerator)target;
        toneGenerator.DebugInitialisaion();

        if (GUILayout.Button("Play Primary Sound"))
        {
            toneGenerator.PlayAudioClipOne();
        }

        if (GUILayout.Button("Play Secondary Sound"))
        {
            toneGenerator.PlayAudioClipTwo();
        }

        if (GUILayout.Button("Play Combined Sounds"))
        {
            toneGenerator.CombineAudioClips();
        }

        if (GUILayout.Button("Play Inserted Sounds"))
        {
            toneGenerator.InsertAudioClips();
        }

        if (GUILayout.Button("Play Keyboard Notes"))
        {
            toneGenerator.PlayKeyboardKeys();
        }
    }
}
