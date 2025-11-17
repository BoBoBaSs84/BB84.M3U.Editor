// Copyright: 2025 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.M3U.Abstractions;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;

using M3UEntry = BB84.M3U.Entry;
using M3UPlaylist = BB84.M3U.Playlist;

namespace BB84.M3U.Editor.Pages;

public partial class Playlist
{
	private M3UPlaylist? playlist;
	private string? errorMessage;
	private string? successMessage;

	/// <summary>
	/// Gets or sets the JavaScript runtime.
	/// </summary>
	[Inject] public IJSRuntime JSRuntime { get; init; } = default!;

	/// <summary>
	/// Gets or sets the navigation manager.
	/// </summary>
	[Inject] public NavigationManager NavigationManager { get; init; } = default!;

	/// <summary>
	/// Gets or sets the serializer.
	/// </summary>
	[Inject] public ISerializer Serializer { get; init; } = default!;

	/// <inheritdoc/>
	protected override void OnInitialized()
	{
		Uri uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
		if (QueryHelpers.ParseQuery(uri.Query).ContainsKey("new"))
			InitializeNewPlaylist();
	}

	private async Task OnFileSelected(InputFileChangeEventArgs e)
	{
		errorMessage = null;
		successMessage = null;
		var file = e.File;
		if (file == null)
		{
			errorMessage = "No file selected.";
			return;
		}

		try
		{
			using var stream = file.OpenReadStream();
			using var reader = new StreamReader(stream);
			string content = await reader.ReadToEndAsync();

			playlist = PlaylistDeserializeFromString(content);
		}
		catch (Exception ex)
		{
			errorMessage = $"Failed to load playlist: {ex.Message}";
		}
	}

	private M3UPlaylist PlaylistDeserializeFromString(string content)
	{
		string tempFile = Path.GetTempFileName();
		File.WriteAllText(tempFile, content);
		M3UPlaylist result = Serializer.Deserialize(tempFile);
		File.Delete(tempFile);
		return result;
	}

	private async Task SavePlaylist()
	{
		if (playlist == null)
			return;

		try
		{
			string m3uContent = Serializer.Serialize(playlist);
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(m3uContent);
			string fileName = "playlist.m3u";

			using var stream = new MemoryStream(bytes);
			using var streamRef = new DotNetStreamReference(stream);

			await JSRuntime.InvokeVoidAsync(
				"downloadFileFromStream",
				fileName,
				streamRef
				);

			successMessage = "Playlist saved and download started.";
		}
		catch (Exception ex)
		{
			errorMessage = $"Failed to save playlist: {ex.Message}";
		}
	}

	private void AddEntry()
	{
		playlist?.Entries.Add(new M3UEntry
		{
			Title = "New Channel",
			FilePath = "",
			Duration = 0,
			Metadata = new()
		});
	}

	private void RemoveEntry(int index)
	{
		if (playlist != null && index >= 0 && index < playlist.Entries.Count)
			playlist.Entries.RemoveAt(index);
	}

	private void InitializeNewPlaylist()
	{
		playlist = new M3UPlaylist();
		errorMessage = null;
		successMessage = null;
	}
}
