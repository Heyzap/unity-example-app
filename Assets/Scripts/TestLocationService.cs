using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Heyzap;

public class TestLocationService : MonoBehaviour
{
    [SerializeField]
    private ScrollingTextArea console;

    [SerializeField]
    private AdManager adManager;

    public IEnumerator Start()
    {
        UnityEngine.Debug.Log ("location service start");
        // First, check if user has location service enabled
        // if (!Input.location.isEnabledByUser){
        //     console.Append("Location disabled by user... quitting");
        //     yield break;
        // }

        // Start service before querying location
        console.Append("Starting location service");
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            console.Append("still initializing location");
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            console.Append("Location Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            console.Append("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            console.Append("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            adManager.UpdateLocation(Input.location.lastData.latitude, Input.location.lastData.longitude, Input.location.lastData.horizontalAccuracy, Input.location.lastData.verticalAccuracy, Input.location.lastData.altitude, Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }
}
