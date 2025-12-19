using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
	public static HighScoreManager Instance { get; private set; }

	[Header("UI")]
	public GameObject submitPanel;
	public TMP_InputField usernameInput;
	public Button submitButton;
	public TMP_Text highscoreListText;

	const string PREFS_KEY = "HighScores_v1";

	HighScoreList highScoreList = new HighScoreList();

	int pendingScore;
	float pendingTime;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		Load();
		if (submitPanel != null) submitPanel.SetActive(false);
	}

	public void PrepareForSubmit(int score, float timeSeconds)
	{
		pendingScore = score;
		pendingTime = timeSeconds;
		if (submitPanel != null) submitPanel.SetActive(true);
		if (usernameInput != null) usernameInput.text = string.Empty;
		if (submitButton != null)
		{
			submitButton.onClick.RemoveAllListeners();
			submitButton.onClick.AddListener(Submit);
		}
	}

	public void Submit()
	{
		string name = "Player";
		if (usernameInput != null && !string.IsNullOrWhiteSpace(usernameInput.text))
		{
			name = usernameInput.text.Trim();
		}

		var entry = new HighScoreEntry
		{
			name = name,
			score = pendingScore,
			timeSeconds = pendingTime,
			date = DateTime.UtcNow.ToString("o")
		};

		highScoreList.entries.Add(entry);
		Save();
		if (submitPanel != null) submitPanel.SetActive(false);
		UpdateHighscoreDisplay();
	}

	void Save()
	{
		try
		{
			var json = JsonUtility.ToJson(highScoreList);
			PlayerPrefs.SetString(PREFS_KEY, json);
			PlayerPrefs.Save();
		}
		catch (Exception e)
		{
			Debug.LogError("Failed to save highscores: " + e);
		}
	}

	void Load()
	{
		var json = PlayerPrefs.GetString(PREFS_KEY, string.Empty);
		if (!string.IsNullOrEmpty(json))
		{
			try
			{
				highScoreList = JsonUtility.FromJson<HighScoreList>(json) ?? new HighScoreList();
			}
			catch
			{
				highScoreList = new HighScoreList();
			}
		}
	}

	public List<HighScoreEntry> GetTop(int count = 10)
	{
		return highScoreList.entries
			.OrderByDescending(e => e.score)
			.ThenBy(e => e.timeSeconds)
			.Take(count)
			.ToList();
	}

	public void UpdateHighscoreDisplay(int count = 10)
	{
		if (highscoreListText == null) return;
		var top = GetTop(count);
		var sb = new StringBuilder();
		int rank = 1;
		foreach (var e in top)
		{
			sb.AppendLine(string.Format("{0}. {1} - {2} pts - {3}", rank, e.name, e.score, FormatTime(e.timeSeconds)));
			rank++;
		}
		highscoreListText.text = sb.ToString();
	}

	string FormatTime(float t)
	{
		int total = Mathf.FloorToInt(t);
		int hours = total / 3600;
		int minutes = (total % 3600) / 60;
		int seconds = total % 60;
		if (hours > 0) return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
		if (minutes > 0) return string.Format("{0:00}:{1:00}", minutes, seconds);
		return string.Format("{0:00}", seconds);
	}

	[Serializable]
	public class HighScoreEntry
	{
		public string name;
		public int score;
		public float timeSeconds;
		public string date;
	}

	[Serializable]
	public class HighScoreList
	{
		public List<HighScoreEntry> entries = new List<HighScoreEntry>();
	}
}
