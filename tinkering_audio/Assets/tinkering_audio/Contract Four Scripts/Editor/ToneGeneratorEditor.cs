using UnityEngine;
using UnityEditor;

/// <summary>
/// In conjunction with the ToneGenerator variables this is used to create a 
/// variety of buttons for a designer to create sounds quickly and effectively
/// </summary>
[CustomEditor(typeof(ToneGenerator))]
public class ToneGeneratorEditor : Editor
{
    /// <summary>
    /// Used to allow the user to invoke the function related to the respective
    /// button which allows them to create sounds using the inspector only
    /// </summary>
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

        if (GUILayout.Button("Primary Echo"))
        {
            toneGenerator.CreatePrimaryEchoClip();
        }
    }
}
