using System.Text;
using System.Security.Cryptography;
using System;

public static class CryptionTool
{
    private static string PwdKey = "UEct5x07OreGowsx";
    private static string ServiceKey = "MIGrAgEAAiEA0pQk";
    private static string LoginTimeStr = "";
    public static string GetA()
    {
        return LoginTimeStr;
    }
    public static void Reset()
    {
        LoginTimeStr = "";
    }
    public static string Encrypt(string encryptStr, string key)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(encryptStr);

        RijndaelManaged rDel = new RijndaelManaged();

        rDel.Key = keyArray;
        rDel.Mode = CipherMode.CBC;
        rDel.Padding = PaddingMode.PKCS7;
        rDel.IV = UTF8Encoding.UTF8.GetBytes(key);

        ICryptoTransform cTransform = rDel.CreateEncryptor();

        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    public static string Decrypt(string decryptStr, string key)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
        byte[] toEncryptArray = Convert.FromBase64String(decryptStr);

        RijndaelManaged rDel = new RijndaelManaged();

        rDel.Key = keyArray;
        rDel.Mode = CipherMode.CBC;
        rDel.Padding = PaddingMode.PKCS7;
        rDel.IV = UTF8Encoding.UTF8.GetBytes(key);

        ICryptoTransform cTransform = rDel.CreateDecryptor();

        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return UTF8Encoding.UTF8.GetString(resultArray);
    }
    public static string GetMonoToken(string a, string b, int t)
    {
        var str = t.ToString();
        var data = "";
        switch (t % 4)
        {
            case 0:
                data = a + "|" + b + "|" + str;
                break;
            case 1:
                data = b + "|" + a + "|" + str;
                break;
            case 2:
                data = str + "|" + a + "|" + b;
                break;
            case 3:
                data = str + "|" + b + "|" + a;
                break;
            default:

                break;
        }
        string mono = Encrypt(data, ServiceKey);
        return mono;
    }
    public static string GetTmpPwd(string a, string b)
    {
        var t = TimeTool.GetUtcTimeInt;
        LoginTimeStr = t.ToString();
        var data = "";
        switch (t % 4)
        {
            case 0:
                data = a + "✟" + b + "✟" + LoginTimeStr;
                break;
            case 1:
                data = b + "✟" + a + "✟" + LoginTimeStr;
                break;
            case 2:
                data = LoginTimeStr + "✟" + a + "✟" + b;
                break;
            case 3:
                data = LoginTimeStr + "✟" + b + "✟" + a;
                break;
            default:
                break;
        }
        string pwd = Encrypt(data, PwdKey);
        return pwd;
    }
}