// Copyright: 2025 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.M3U.Tests.Properties;

namespace BB84.M3U.Tests;

[TestClass]
public sealed class PlaylistTests
{
	private static string s_testFilePath = string.Empty;

	[AssemblyInitialize]
	public static void AssemblyInit(TestContext context)
	{
		s_testFilePath = Path.Combine(Environment.CurrentDirectory, "test_playlist.m3u");
		File.WriteAllText(s_testFilePath, Resources.IPTVM3U);
	}

	[AssemblyCleanup]
	public static void AssemblyCleanup()
	{
		File.Delete(s_testFilePath);
		s_testFilePath = string.Empty;
	}

	[TestMethod]
	public void DeserializeTest()
	{
		Serializer serializer = new();
		Playlist playlist = serializer.Deserialize(s_testFilePath);

		Assert.IsNotEmpty(playlist.Entries);
		Assert.AreEqual(-1, playlist.Entries[0].Duration);
		Assert.AreEqual("Das Erste HD", playlist.Entries[0].Title);
		Assert.AreEqual("https://daserste-live.ard-mcdn.de/daserste/live/hls/de/master.m3u8", playlist.Entries[0].FilePath);
	}

	[TestMethod]
	public void SerializeTest()
	{
		Playlist playlist = new()
		{
			Cache = 1000,
			Deinterlace = Deinterlace.Blend,
			UrlTvg = "https://example.com/epg.xml",
			Refresh = 3600
		};
		playlist.Entries.Add(new Entry
		{
			Duration = -1,
			Title = "The First Channel HD",
			Grouping = "Germany",
			FilePath = "https://example.com/example.m3u8",
			Metadata = new Metadata
			{
				TvgId = "1",
				TvgName = "TheFirstChannelHD",
				TvgLogo = "https://example.com/example.png",
				GroupTitle = "Germany",
				GroupId = "1"
			}
		});

		Serializer serializer = new();
		string fileContent = serializer.Serialize(playlist);

		Assert.Contains("#EXTM3U url-tvg=\"https://example.com/epg.xml\" cache=\"1000\" deinterlace=\"1\" refresh=\"3600\"", fileContent);
		Assert.Contains("#EXTINF:-1 tvg-id=\"1\" tvg-name=\"TheFirstChannelHD\" tvg-logo=\"https://example.com/example.png\" group_id=\"1\" group-title=\"Germany\", The First Channel HD", fileContent);
		Assert.Contains("#EXTGRP:Germany", fileContent);
		Assert.Contains("https://example.com/example.m3u8", fileContent);
	}
}
