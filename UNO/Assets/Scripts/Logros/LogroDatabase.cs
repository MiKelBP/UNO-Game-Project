using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LogroDatabase", menuName = "Logros/LogroDatabase")]
public class LogroDatabase : ScriptableObject {
    
    public Logro[] logros;

    public int LogroCount
    {
        get{ return logros.Length; }
    }

    public Logro GetLogro(int index)
    {
        return logros[index];
    }

    public void PurchaseLogro (int index)
	{
		logros [index].isPurchased = true;
	}

	public void ReinicioPurchaseLogro ()
	{
		for (int i=0; i<10; i++){
			logros [i].isPurchased = false;
		}
	}

}
