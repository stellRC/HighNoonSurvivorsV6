using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField]
    private Slider masterSlider;

    [SerializeField]
    private Slider effectsSlider;

    [SerializeField]
    private Slider musicSlider;

    [Header("Display Settings")]
    [SerializeField]
    private TMP_Dropdown resolutionDropdown;

    [SerializeField]
    private Toggle vSyncToggle;

    [SerializeField]
    private Toggle windowedToggle;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    List<string> options;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    void Start()
    {
        FilterResolutions();
    }

    private void FilterResolutions()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        options = new();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;

        foreach (Resolution value in resolutions)
        {
            if ((float)value.refreshRateRatio.value == currentRefreshRate)
            {
                filteredResolutions.Add(value);
            }
        }

        // Sort resolutions by size
        filteredResolutions.Sort(
            (a, b) =>
            {
                if (a.width != b.width)
                    return b.width.CompareTo(a.width);
                else
                    return b.height.CompareTo(a.height);
            }
        );

        // Save resolution as a string for dropdown
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption =
                filteredResolutions[i].width + "x" + filteredResolutions[i].height;
            CommonResolutions(resolutionOption);

            if (
                filteredResolutions[i].width == Screen.width
                && filteredResolutions[i].height == Screen.height
                && (float)filteredResolutions[i].refreshRateRatio.value == currentRefreshRate
            )
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex = 0;
        resolutionDropdown.RefreshShownValue();
        SetResolution(currentResolutionIndex);
    }

    // Add resolutions to dropdown if they will work on OS and don't break the game
    private void CommonResolutions(string resolutionOption)
    {
        switch (resolutionOption)
        {
            case "1920x1080":
                options.Add(resolutionOption);
                break;
            case "1366x768":
                options.Add(resolutionOption);
                break;
            case "2540x1440":
                options.Add(resolutionOption);
                break;
            case "800x600":
                options.Add(resolutionOption);
                break;
            case "3840x1080":
                options.Add(resolutionOption);
                break;
        }
    }

    // Set resolutions in dropdown
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    // Create dynamic variable for fullscreen that will set itself automatically when toggled
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
