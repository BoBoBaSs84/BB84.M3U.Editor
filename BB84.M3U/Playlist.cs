// Copyright: 2025 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.M3U;

/// <summary>
/// Represents an M3U playlist, which can contain multiple entries with associated metadata.
/// </summary>
public sealed class Playlist
{
	/// <summary>
	/// Gets or sets the URL for the TV guide.
	/// </summary>
	public string? UrlTvg { get; set; }

	/// <summary>
	/// Gets or sets the cache period in milliseconds. Set to 0 if not specified.
	/// </summary>
	public int Cache { get; set; }

	/// <summary>
	/// Gets or sets the deinterlace method.
	/// </summary>
	public Deinterlace Deinterlace { get; set; } = Deinterlace.None;

	/// <summary>
	/// Gets or sets the refresh period in seconds.
	/// </summary>
	public int Refresh { get; set; }

	/// <summary>
	/// Gets the list of entries in the playlist.
	/// </summary>
	public List<Entry> Entries { get; } = [];
}
