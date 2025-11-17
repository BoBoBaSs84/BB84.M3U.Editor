// Copyright: 2025 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.M3U.Tests;

[TestClass]
public sealed class EntryTests
{
	[TestMethod]
	public void ConstructorShouldInitializePropertiesCorrect()
	{
		string title = "Sample Title";
		string filePath = "http://example.com/media.mp3";
		Entry? entry;

		entry = new Entry()
		{
			Title = title,
			FilePath = filePath
		};

		Assert.AreEqual(title, entry.Title);
		Assert.AreEqual(filePath, entry.FilePath);
		Assert.AreEqual(-1, entry.Duration);
		Assert.IsNull(entry.Grouping);
		Assert.IsNull(entry.Metadata);
	}
}
