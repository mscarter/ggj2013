using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateDialogEditor : ScriptableObject {
	
	[MenuItem ("GameObject/Create Other/Character Tree")]
	public static void MenuCreateCharacterTree()
	{
//        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
 
		GameObject characterObject = new GameObject("Character Name", typeof(ClubPatron));
		var patron = characterObject.GetComponent<ClubPatron>();
		
		patron.initialQuestion1 = CreateDialog(1, characterObject.transform, null);
		patron.initialQuestion2 = CreateDialog(1, characterObject.transform, null);
		patron.initialQuestion3 = CreateDialog(1, characterObject.transform, null);
	}
	
	static CharacterDialog CreateDialog(int depth, Transform parent, CharacterDialog firstDialog)
	{
		GameObject dialogObject = new GameObject("Character Dialog", typeof(CharacterDialog));
		dialogObject.transform.parent = parent;
		
		var dialog = dialogObject.GetComponent<CharacterDialog>();
		
		if (depth == 1)
		{
			firstDialog = dialog;
		}
		
		if (depth < 3)
		{
			dialog.nextDialog = CreateDialog(depth + 1, dialogObject.transform, firstDialog);
		}
		else
		{
			//dialog.nextDialog = firstDialog;
		}
		return dialog;
	}
}
