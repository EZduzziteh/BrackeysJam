using TMPro;
using UnityEngine;

public class StatLoader : MonoBehaviour
{

    public TextMeshProUGUI murdersText;
    public TextMeshProUGUI killOMetersText;
    public TextMeshProUGUI timeVictimsAliveText;


    private void Start()
    {
        LoadStats();
    }

    public void LoadStats()
    {
        killOMetersText.text = StatTracker.Instance.distanceTravelled.ToString() + " Kill-O-Meters";
        murdersText.text =  StatTracker.Instance.totalPassengersCollected.ToString() + " Poor Unforunate Souls";
        timeVictimsAliveText.text = StatTracker.Instance.totalPassengerTime.ToString() + " Very Long Seconds";
    }
}
