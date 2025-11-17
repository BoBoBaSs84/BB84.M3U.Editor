// Copyright: 2025 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.M3U;

/// <summary>
/// Represents a media track entry in an M3U playlist.
/// </summary>
public sealed class Entry
{
	/// <summary>
	/// Gets or sets the duration of the track in seconds.
	/// </summary>
	/// <remarks>
	/// The default value is -1, indicating an unknown duration.
	/// </remarks>
	public int Duration { get; set; } = -1;

	/// <summary>
	/// Gets or sets the title of the track.
	/// </summary>
	public required string Title { get; set; }

	/// <summary>
	/// Gets or sets the path to the media file.
	/// </summary>
	public required string FilePath { get; set; }

	/// <summary>
	/// Gets or sets the named grouping of the track.
	/// </summary>
	public string? Grouping { get; set; }

	/// <summary>
	/// Gets or sets the metadata associated with the track.
	/// </summary>
	public Metadata? Metadata { get; set; }
}
