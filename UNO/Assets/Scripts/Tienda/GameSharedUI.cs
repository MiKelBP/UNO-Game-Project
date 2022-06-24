using UnityEngine;
using TMPro;

public class GameSharedUI : MonoBehaviour
{
	#region Singleton class: GameSharedUI

	public static GameSharedUI Instance;

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		}
	}

	#endregion

	[SerializeField] TMP_Text[] coinsUIText;
	private static int monedas; 

	public void Start ()
	{
		monedas =  GameDataManager.GetCoins();
		UpdateCoinsUIText ();
	}

	public void UpdateCoinsUIText ()
	{
		for (int i = 0; i < coinsUIText.Length; i++) {
			SetCoinsText (coinsUIText [i], monedas);
		}
		controladorLogro cL = GameObject.Find("controladorLogros").GetComponent<controladorLogro>();
		cL.consigue5000Monedas();
		cL.consigue1000Monedas();
	}

	void SetCoinsText (TMP_Text textMesh, int value)
	{
		// if (value >= 1000000)...
		// .....

		if (value >= 1000)
			textMesh.text = string.Format ("{0}K", (value), GetFirstDigitFromNumber (value));
		else
			textMesh.text = value.ToString ();
	}

	int GetFirstDigitFromNumber (int num)
	{
		return int.Parse (num.ToString () [0].ToString ());
	}
}
