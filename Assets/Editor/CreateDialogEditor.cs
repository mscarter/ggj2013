using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateDialogEditor : ScriptableObject {
	
	[MenuItem ("GameObject/Create Other/Character Dialog")]
	public static void MenuCreateCharacterDialog()
	{
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
 
        foreach(Transform transform in transforms)
        {
            GameObject newChild = new GameObject("Character Dialog", typeof(CharacterDialog));
            newChild.transform.parent = transform;
			newChild.transform.localPosition = Vector3.zero;
        }
	}
}
