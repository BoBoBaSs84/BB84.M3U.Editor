// Copyright: 2025 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.M3U;

/// <summary>
/// Represents metadata associated with an M3U entry.
/// </summary>
public sealed class Metadata
{
  /// <summary>
  /// Indicates whether the content is censored.
  /// </summary>
	public bool Censored { get; set; }

  /// <summary>
  /// Get or set the TV guide ID.
  /// </summary>
  public string? TvgId { get; set; }

  /// <summary>
  /// Get or set the TV guide name.
  /// </summary>
  public string? TvgName { get; set; }

  /// <summary>
  /// Get or set the TV guide logo URL.
  /// </summary>
  public string? TvgLogo { get; set; }

  /// <summary>
  /// Get or set the group ID.
  /// </summary>
  public string? GroupId { get; set; }

  /// <summary>
  /// Get or set the group title.
  /// </summary>
  public string? GroupTitle { get; set; }
}
