using UnityEngine;



public class TouchDeviceChecker : MonoBehaviour
{
    public void IsAndroidCheck()
    {
        PlayerData[] playerDatas = Resources.LoadAll<PlayerData>("");

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            foreach (PlayerData playerData in playerDatas)
            {
                playerData.isAndroidControl = true;
            }

        }
        else
        {
            foreach (PlayerData playerData in playerDatas)
            {
                playerData.isAndroidControl = false;
            }
        }
    }
}