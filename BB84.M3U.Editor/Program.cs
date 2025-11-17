// Copyright: 2025 Robert Peter Meyer
// License: MIT
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
using BB84.M3U.Abstractions;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BB84.M3U.Editor;

internal sealed class Program
{
	public static async Task Main(string[] args)
	{
		WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
		builder.RootComponents.Add<App>("#app");
		builder.RootComponents.Add<HeadOutlet>("head::after");

		builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
		builder.Services.AddSingleton<ISerializer, Serializer>();

		await builder.Build()
			.RunAsync();
	}
}
