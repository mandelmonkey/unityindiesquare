using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NBitcoin;
using System;
using UnityEngine.UI;
using LitJson;
public class GetAddress : MonoBehaviour {
	public Text resultsText;
	public string urlscheme;
	private string randomMessage;
    // Use this for initialization

    public void LinkAddressIndieSquare()
	{
		if (urlscheme != "") {
			callUrlScheme("indiewallet","inc.lireneosoft.counterparty");
		} else {

			Debug.Log ("please set urlscheme");
		}
	}
    public void LinkAddressBookOfOrbs()
    {
		if (urlscheme != "") {
			callUrlScheme("orbbook","inc.indiesquare.orbbook");
		} else {

			Debug.Log ("please set urlscheme");
		}
    }

	public void callUrlScheme(string appScheme,string appID){

		Guid myGUID = Guid.NewGuid();
		randomMessage = myGUID.ToString(); //generate random msg for the apps to sign and verify address

		bool fail = false;

			#if UNITY_IPHONE
			    Application.OpenURL(appScheme+"://x-callback-url/getaddress?msg="+randomMessage+"&x-success="+ urlscheme);
			#endif
			#if UNITY_ANDROID
			    AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			    AndroidJavaObject ca = up.GetStatic<AndroidJavaObject> ("currentActivity");
			    AndroidJavaObject packageManager = ca.Call<AndroidJavaObject> ("getPackageManager");
			    AndroidJavaObject launchIntent = null;

			    launchIntent = packageManager.Call<AndroidJavaObject> ("getLaunchIntentForPackage", appID);
			    try {
			        launchIntent.Call<AndroidJavaObject> ("putExtra", "source", "x-callback-url/getaddress?x-success=" + urlscheme + "&msg=" + randomMessage);
			    } catch (System.Exception e) {
			        fail = true;
			    }

			    if (fail) {
			        Debug.Log ("app not found");
			    } else {
			        ca.Call ("startActivity", launchIntent);
			    }
			    up.Dispose ();
			    ca.Dispose ();
			    packageManager.Dispose ();
			    launchIntent.Dispose ();
			#endif
	}
		
	void verifyAddress(string url){ 
        if (url != null) { 
			
			url = url.Replace (urlscheme+"://", "");
			url = url.Replace ("sendaddress?", "");
			url = url.Replace ("address=", "");
			url = url.Replace ("&msg=", "PARSE");
			url = url.Replace ("&sig=", "PARSE");

			string[] param = url.Split (new[] { "PARSE" }, StringSplitOptions.None);

			string address = "";
			string signature = "";

			if (param.Length > 0) { address = param [0]; }

			if (param.Length > 2) {  signature = param [2]; } 

			try
			{
				BitcoinPubKeyAddress addrs = new BitcoinPubKeyAddress (address);
 
				if (addrs.VerifyMessage (randomMessage, signature)) {

					Debug.Log ("Address verified!");

					StartCoroutine(getAddressBalance(address));

				} else{

					Debug.Log ("Address not verified!");

				}
			}
			catch(System.FormatException e)
			{
				Debug.Log("address not valid");


			}
		}


	}
	private IEnumerator getAddressBalance(string address)
	{
		resultsText.text = "getting address balance..";
		var url = "https://api.indiesquare.me/v2/addresses/"+ address + "/balances";//add apikey if you have it e.g ?X-Api-Key=b14d8ae0edxdjtb9e3dt08iw

		WWW www = new WWW(url);

		yield return www;


		if (www.error != null)
		{
			 

			Debug.Log(www.error);

		}
		else
		{

		 
			JsonData data = JsonMapper.ToObject(www.text);

			resultsText.text = "User owns\n";

			foreach (JsonData aToken in data)
			{

				string tokenName = aToken["token"].ToString();

				string tokenBalanceString = aToken["balance"].ToString();

				resultsText.text += tokenName+" "+tokenBalanceString+"\n";

			}


	}
}
	void OnApplicationPause(bool pauseStatus)
	{
		if(pauseStatus == false){ 
			#if UNITY_IPHONE 
				string passedData = PlayerPrefs.GetString("url_scheme");
				if (passedData != null) {
					PlayerPrefs.DeleteKey("url_scheme");
					Debug.Log ("url is " + passedData);
					verifyAddress(passedData);
				}

			#endif
			#if UNITY_ANDROID 
				string query = CustomUrlSchemeAndroid.GetLaunchedUrlQuery();
				if (query != null)
				{ 
					verifyAddress(query);
				} 
			#endif
			}
	}
   
}
