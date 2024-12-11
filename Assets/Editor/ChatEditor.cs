using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(Chat))]
    public class ChatEditor : UnityEditor.Editor
    {
        private const string ChatOwnerLabel = "Chat Owner";

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (target is Chat chat)
            {
                var previousOwner = chat.Owner;

                // Display an input field for the Chat Owner
                string newOwner = EditorGUILayout.TextField(ChatOwnerLabel, previousOwner);

                if (newOwner != previousOwner)
                {
                    chat.Owner = newOwner;
                    EditorUtility.SetDirty(target);
                }
            }
        }
    }
}
