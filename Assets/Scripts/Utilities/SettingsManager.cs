using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Display Settings")]
    [SerializeField]
    private TMP_Dropdown _resolutionDropdown;
    private Resolution[] _resolutions;
    private List<Resolution> _filteredResolutions;
    private List<string> _options;
    private float _currentRefreshRate;
    private int _currentResolutionIndex = 0;

    void Start()
    {
        FilterResolutions();
    }

    private void FilterResolutions()
    {
        _resolutions = Screen.resolutions;
        _filteredResolutions = new List<Resolution>();
        _options = new();

        _resolutionDropdown.ClearOptions();
        _currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;

        foreach (Resolution value in _resolutions)
        {
            if ((float)value.refreshRateRatio.value == _currentRefreshRate)
            {
                _filteredResolutions.Add(value);
            }
        }

        // Sort resolutions by size
        _filteredResolutions.Sort(
            (a, b) =>
            {
                if (a.width != b.width)
                    return b.width.CompareTo(a.width);
                else
                    return b.height.CompareTo(a.height);
            }
        );

        // Save resolution as a string for dropdown
        for (int i = 0; i < _filteredResolutions.Count; i++)
        {
            string resolutionOption =
                _filteredResolutions[i].width + "x" + _filteredResolutions[i].height;
            CommonResolutions(resolutionOption);

            if (
                _filteredResolutions[i].width == Screen.width
                && _filteredResolutions[i].height == Screen.height
                && (float)_filteredResolutions[i].refreshRateRatio.value == _currentRefreshRate
            )
            {
                _currentResolutionIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(_options);
        _resolutionDropdown.value = _currentResolutionIndex = 0;
        _resolutionDropdown.RefreshShownValue();
        SetResolution(_currentResolutionIndex);
    }

    // Add resolutions to dropdown if they will work on OS and don't break the game
    private void CommonResolutions(string resolutionOption)
    {
        switch (resolutionOption)
        {
            case "1920x1080":
                _options.Add(resolutionOption);
                break;
            case "1366x768":
                _options.Add(resolutionOption);
                break;
            case "2540x1440":
                _options.Add(resolutionOption);
                break;
            case "800x600":
                _options.Add(resolutionOption);
                break;
            case "3840x1080":
                _options.Add(resolutionOption);
                break;
        }
    }

    // Set resolutions in dropdown
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    // Create dynamic variable for fullscreen that will set itself automatically when toggled
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
