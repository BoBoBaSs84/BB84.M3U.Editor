// Copyright: 2025 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
namespace BB84.M3U.Tests;

[TestClass]
public sealed class MetadataTests
{
	[TestMethod]
	public void MetadataDefaultValuesAreCorrect()
	{
		Metadata? metadata;

		metadata = new Metadata();

		Assert.IsFalse(metadata.Censored, "Censored should be false by default.");
		Assert.IsNull(metadata.TvgId, "TvgId should be null by default.");
		Assert.IsNull(metadata.TvgName, "TvgName should be null by default.");
		Assert.IsNull(metadata.TvgLogo, "TvgLogo should be null by default.");
		Assert.IsNull(metadata.GroupId, "GroupId should be null by default.");
		Assert.IsNull(metadata.GroupTitle, "GroupTitle should be null by default.");
	}
}
