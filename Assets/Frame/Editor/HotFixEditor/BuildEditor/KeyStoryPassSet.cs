using UnityEditor;
[InitializeOnLoad]
public class KeyStoryPassSet
{
    static KeyStoryPassSet()
    {
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = "Assets/baseframewxold/Frame/keystore/yuzi_android_key.jks";
        PlayerSettings.Android.keyaliasName = "yuzi";
        PlayerSettings.Android.keyaliasPass = "yuzi123";
        PlayerSettings.Android.keystorePass = "yuzi123";
    }
}