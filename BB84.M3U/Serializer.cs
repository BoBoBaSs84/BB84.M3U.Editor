// Copyright: 2025 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using System.Text;

using BB84.M3U.Abstractions;

namespace BB84.M3U;

/// <summary>
/// Represents a serializer for M3U playlists, providing methods to serialize and deserialize playlist data.
/// </summary>
public sealed class Serializer : ISerializer
{
	private static readonly string[] Separator = ["\r\n", "\n"];

	/// <inheritdoc/>
	public Playlist Deserialize(string filePath)
	{
		byte[] fileContent = File.ReadAllBytes(filePath);
		return Deserialize(fileContent);
	}

	/// <inheritdoc/>
	public Playlist Deserialize(string[] fileLines)
	{
		Playlist playlist = new();

		if (fileLines.Length == 0 || !fileLines[0].StartsWith("#EXTM3U", StringComparison.OrdinalIgnoreCase))
			throw new InvalidDataException("The file is not a valid M3U file.");

		string extM3uLine = fileLines[0];
		string[] attributes = extM3uLine.Split([' '], StringSplitOptions.RemoveEmptyEntries);
		foreach (string attribute in attributes)
		{
			if (attribute.StartsWith("url-tvg=", StringComparison.OrdinalIgnoreCase))
				playlist.UrlTvg = attribute.Split('=')[1].Trim('"');
			else if (attribute.StartsWith("refresh=", StringComparison.OrdinalIgnoreCase))
			{
				if (int.TryParse(attribute.Split('=')[1].Trim('"'), out int refresh))
					playlist.Refresh = refresh;
			}
			else if (attribute.StartsWith("cache=", StringComparison.OrdinalIgnoreCase))
			{
				if (int.TryParse(attribute.Split('=')[1].Trim('"'), out int cache))
					playlist.Cache = cache;
			}
		}

		Entry? currentEntry = null;

		foreach (string line in fileLines.Skip(1)) // Skip the #EXTM3U line
		{
			if (line.StartsWith("#EXTINF:", StringComparison.OrdinalIgnoreCase))
			{
				string metadataLine = line[8..];

				string[] metadataParts = metadataLine.Split(' ', '=', ',');

				int duration = default;
				if (int.TryParse(metadataParts.FirstOrDefault() ?? string.Empty, out int parsed))
					duration = parsed;

				Metadata metadata = new()
				{
					Censored = line.Contains("censored=\"1\"")
				};

				if (metadataLine.Contains("tvg-id"))
					metadata.TvgId = GetMetadataValue(metadataLine, "tvg-id");
				if (metadataLine.Contains("tvg-name"))
					metadata.TvgName = GetMetadataValue(metadataLine, "tvg-name");
				if (metadataLine.Contains("tvg-logo"))
					metadata.TvgLogo = GetMetadataValue(metadataLine, "tvg-logo");
				if (metadataLine.Contains("group_id"))
					metadata.GroupId = GetMetadataValue(metadataLine, "group_id");
				if (metadataLine.Contains("group-title"))
					metadata.GroupTitle = GetMetadataValue(metadataLine, "group-title");

				int titleIndex = metadataLine.LastIndexOf(',');
				string title = titleIndex >= 0 ? metadataLine[(titleIndex + 1)..].Trim() : string.Empty;

				currentEntry = new Entry
				{
					Duration = duration,
					Title = title,
					FilePath = string.Empty,
					Metadata = metadata
				};
			}
			else if (line.StartsWith("#EXTGRP:", StringComparison.OrdinalIgnoreCase))
			{
				if (currentEntry is not null)
					currentEntry.Grouping = line[8..];
			}
			else if (!line.StartsWith("#", StringComparison.OrdinalIgnoreCase))
			{
				if (currentEntry is not null)
				{
					currentEntry.FilePath = line.Trim();
					playlist.Entries.Add(currentEntry);
				}
				currentEntry = null;
			}
		}

		return playlist;
	}

	/// <inheritdoc/>
	public Playlist Deserialize(byte[] fileContent)
	{
		string contentString = Encoding.UTF8.GetString(fileContent);
		string[] fileLines = contentString.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
		return Deserialize(fileLines);
	}

	/// <inheritdoc/>
	public string Serialize(Playlist playlist)
	{
		StringBuilder sb = new();
		sb.Append("#EXTM3U");
		if (playlist.UrlTvg is not null)
			sb.Append($" url-tvg=\"{playlist.UrlTvg}\"");
		if (playlist.Cache > 0)
			sb.Append($" cache=\"{playlist.Cache}\"");
		if (playlist.Deinterlace is not Deinterlace.None)
			sb.Append($" deinterlace=\"{(int)playlist.Deinterlace}\"");
		if (playlist.Refresh > 0)
			sb.Append($" refresh=\"{playlist.Refresh}\"");
		sb.AppendLine();

		foreach (Entry entry in playlist.Entries)
		{
			if (entry.Metadata != null)
			{
				sb.Append($"#EXTINF:{entry.Duration} ");
				if (entry.Metadata.Censored)
					sb.Append("censored=\"1\" ");
				if (entry.Metadata.TvgId is not null)
					sb.Append($"tvg-id=\"{entry.Metadata.TvgId}\" ");
				if (entry.Metadata.TvgName is not null)
					sb.Append($"tvg-name=\"{entry.Metadata.TvgName}\" ");
				if (entry.Metadata.TvgLogo is not null)
					sb.Append($"tvg-logo=\"{entry.Metadata.TvgLogo}\" ");
				if (entry.Metadata.GroupId is not null)
					sb.Append($"group_id=\"{entry.Metadata.GroupId}\" ");
				if (entry.Metadata.GroupTitle is not null)
					sb.Append($"group-title=\"{entry.Metadata.GroupTitle}\", ");
				sb.AppendLine(entry.Title);
			}

			if (entry.Grouping is not null)
				sb.AppendLine($"#EXTGRP:{entry.Grouping}");

			sb.AppendLine(entry.FilePath);
		}

		return sb.ToString();
	}

	private static string GetMetadataValue(string metadataLine, string key)
	{
		string startString = metadataLine[metadataLine.IndexOf(key, StringComparison.OrdinalIgnoreCase)..];
		int indexQuoteOne = startString.IndexOf('"');
		int indexQuoteTwo = startString.IndexOf('"', indexQuoteOne + 1);
		return startString.Substring(indexQuoteOne + 1, indexQuoteTwo - 1 - indexQuoteOne);
	}
}
