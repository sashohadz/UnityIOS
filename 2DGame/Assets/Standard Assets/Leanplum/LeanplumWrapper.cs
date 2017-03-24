	// Copyright 2014, Leanplum, Inc.

using LeanplumSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanplumWrapper : MonoBehaviour
{
    public string AppID;
    public string ProductionKey;
    public string DevelopmentKey;
    public string AppVersion;

	private Var<string> welcomeMessage;

	void Awake()
	{
		if (Application.isEditor)
		{
			LeanplumFactory.SDK = new LeanplumNative();
		}
		else
		{
			// NOTE: Currently, the native iOS and Android SDKs do not support Unity Asset Bundles.
			// If you require the use of asset bundles, use LeanplumNative on all platforms.
			#if UNITY_IPHONE
			LeanplumFactory.SDK = new LeanplumIOS();
			#elif UNITY_ANDROID
			LeanplumFactory.SDK = new LeanplumAndroid();
			#else
			LeanplumFactory.SDK = new LeanplumNative();
            #endif
        }
    }
    
    void Start()
    {
		welcomeMessage = Var<string>.Define("welcomeMessage", "Welcome to Leanplum!");
		Var<bool> testBoolVariable = Var<bool>.Define ("testBoolVariable", true);

        DontDestroyOnLoad(this.gameObject);

		SocketUtilsFactory.Utils = new SocketUtils();
        
        if (!string.IsNullOrEmpty(AppVersion))
        {
            Leanplum.SetAppVersion(AppVersion);
        }
        if (string.IsNullOrEmpty(AppID) || string.IsNullOrEmpty(ProductionKey) || string.IsNullOrEmpty(DevelopmentKey))
        {
            Debug.LogError("Please make sure to enter your AppID, Production Key, and " +
                           "Development Key in the Leanplum GameObject inspector before starting.");
        }


		Leanplum.SetDeviceId ("23-March-D001");
//        if (Debug.isDebugBuild)
//        {
            Leanplum.SetAppIdForDevelopmentMode(AppID, DevelopmentKey);
//        }
//        else
//        {
//            Leanplum.SetAppIdForProductionMode(AppID, ProductionKey);
//        }
			
		#if UNITY_IPHONE
		Leanplum.RegisterForIOSRemoteNotifications();
		#elif UNITY_ANDROID
		Leanplum.SetGcmSenderId(Leanplum.LeanplumGcmSenderId);
		#endif


		Leanplum.VariablesChanged += delegate {
			Debug.Log("Variables Changed");
			Debug.Log(welcomeMessage.Value);
			Debug.Log(testBoolVariable.Value);
		};
			
        Leanplum.Start();
    }

// TEST CASE 01
//
// Call forceContent update after setUserId with auto generated userId.
// Each time Update() is called new userId is generated.
// WARNING:
// Will generate a lot of new userIds in the app if the app is run for longer time.
//	void Update()
//	{
//		if (Leanplum.HasStarted)
//		{
//			Debug.Log("Started Slow Stuff");
//
//			Leanplum.Track("Challenges");
//			Leanplum.SetUserId(System.Guid.NewGuid().ToString());
//			Leanplum.ForceContentUpdate();
//		}
//	}
}
