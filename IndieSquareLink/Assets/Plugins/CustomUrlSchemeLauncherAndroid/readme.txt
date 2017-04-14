//////Custom Url Scheme Launcher Android//////


********** How To Use ***********

This asset is very simple.
Please use this way.

1. Set the custom scheme name in "your.scheme.name" of "Plugins/Android/AndroidManifest.xml"

*If you are using the AndroidManifest.xml in the other Asset,
 Don't import the AndroidManifest.xml of this Asset.
 Please copy and paste the code of Manifest.xml in "Plugins/CustomUrlSchemeLauncherAndroid" folder.
 
2. You can launch the app from a custom link scheme that has been set.

3. You can get the param in these methods after launched the app via custom URL scheme.

CustomUrlSchemeAndroid.cs

// return URL full string. or null.
public static string GetLaunchedUrl(bool clearDataAfterGet = true)

// return URL scheme string. or null.
public static string GetLaunchedUrlScheme(bool clearDataAfterGet = true)

// return URL host string. or null.
public static string GetLaunchedUrlHost(bool clearDataAfterGet = true)

// return URL path string. or null.
public static string GetLaunchedUrlPath(bool clearDataAfterGet = true)

// return URL query string. or null.
public static string GetLaunchedUrlQuery(bool clearDataAfterGet = true)

*If you call the same method twice in a row, the second time you can not get the data.
To get the data in two or more times, please set the "false" argument 1 of get method.

*********************************


********** Other Feature ********

Package installed checker for Android
It can check whether the package is installed on the Android device.
Please use this as >> "CustomUrlSchemeAndroid.IsPackageInstalled("your.package.name");"

*********************************


Please contact us if any questions.
MailTo:nishioka-h@westhillapps.com
