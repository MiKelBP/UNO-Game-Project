using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogrosManager : MonoBehaviour
{
    public Button[] buttons;
	public Button[] buttonsInfo;
    public LogroDatabase logroDB;

    public void Start()
    {
        logroDB.ReinicioPurchaseLogro();
        GenerarLogros();
		
    }
    public void GenerarLogros ()
	{	
		//Loop throw save purchased items and make them as purchased in the Database array
		int nLogros =  LogroDataManager.GetAllPurchasedLogros ().Count;
		for (int i = 0; i < nLogros; i++) {
			int purchasedCharacterIndex = LogroDataManager.GetPurchasedLogro (i);
			logroDB.PurchaseLogro (purchasedCharacterIndex);
		}

		//Generate Items
		for (int i = 0; i < logroDB.LogroCount; i++) {

			Logro logro = logroDB.GetLogro (i);

			if (logro.isPurchased) {
            }
			else{
				buttons[i].image.color = Color.gray;
				buttonsInfo[i].image.color = Color.gray;
			}
		}
	}
    
}
