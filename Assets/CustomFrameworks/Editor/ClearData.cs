using UnityEngine;
using UnityEditor;

public class ClearData : MonoBehaviour {

		[MenuItem ("Custom Options/ClearPrefs")]
		static void ClearPrefs () {
				PlayerPrefs.DeleteAll ();
		}


}
