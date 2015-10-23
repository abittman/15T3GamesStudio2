using UnityEngine;
using System.Collections;
using GUI3D;
using UnityEngine.UI;
/*	Used for easier control of characters
 * 
 * 
 * */
namespace DialogueSystem {
	[ExecuteInEditMode]
	public class CharacterSpawner : MonoBehaviour {
		[Header("Actions")]
		[Tooltip("Spawns a character")]
		public bool IsSpawnCharacter;
		[Tooltip("Removes character.")]
		public bool IsDeleteCharacter;
		[Tooltip("Reloads the text file from resources.")]
		public bool IsReloadData;
		
		[Header("Prefabs")]
		public GameObject MyBodyPrefab;
		public GameObject MyGui3drefab;
		private GameObject MyBodySpawn;
		private GameObject MyGuiSpawn;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			if (IsSpawnCharacter) {
				IsSpawnCharacter = false;
				MyBodySpawn = (GameObject) Instantiate(MyBodyPrefab, transform.position, transform.rotation);
				MyGuiSpawn = (GameObject) Instantiate(MyGui3drefab, transform.position, transform.rotation);
				MyBodySpawn.transform.SetParent(transform);
				MyBodySpawn.name = name;
				MyGuiSpawn.transform.SetParent(transform);
				MyGuiSpawn.name = "Gui3d";
				// link up stuff
				Character MyCharacter = MyBodySpawn.GetComponent<Character>();
				SpeechHandler MySpeech = MyBodySpawn.GetComponent<SpeechHandler>();
				//Debug.LogError("Blarg: "  + MyGuiSpawn.transform.GetChild(0).GetChild(1).name);
				MySpeech.MyDialogueText = MyGuiSpawn.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>();
				// link these to the main body
				Follower MyFollower = MyGuiSpawn.GetComponent<Follower>();
				Billboard MyBillboard = MyGuiSpawn.GetComponent<Billboard>();
				MyFollower.TargetCharacter = MyBodySpawn;
				MyBillboard.TargetCharacter = MyBodySpawn;
			}
			
			if (IsDeleteCharacter) {
				IsDeleteCharacter = false;
				for (int i = transform.childCount-1; i >= 0; i--) {
					DestroyImmediate (transform.GetChild(i).gameObject);
				}
				//DestroyImmediate (MyBodySpawn);
				//DestroyImmediate (MyGuiSpawn);
			}
		}
	}
}