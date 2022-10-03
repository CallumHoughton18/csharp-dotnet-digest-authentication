# Digest Authentication in .NET - An Example

**UPDATE (03/10/2022): .NET 6 should now successfully do digest authentication, [as kindly pointed by someone in the example repository](https://github.com/CallumHoughton18/csharp-dotnet-digest-authentication/issues/1). Should save you the hassle of doing it manually, like in this repository!**

This project contains an extension method in `HttpClientExtensions` that should work out
of the box for basic digest authentication, and be a good starting point to expand upon.

I couldn't personally find a way within .NET to handle digest authentication, and the ways that
were documented flat out didn't work. 

I might release this as a Nuget package if I get the time.

## Example Usage

The integration test in `HttpClientExtensionTests` provides the best example usage of
the simple extension method:

```c#
var client = new HttpClient();
client.BaseAddress = new Uri("https://httpbin.org");
var request = new HttpRequestMessage(HttpMethod.Get, "/digest-auth/auth/username/password");
request.Headers.Add("Accept", "*/*");
request.Headers.Add("User-Agent", "HttpClientDigestAuthTester");
request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
request.Headers.Add("Connection", "keep-alive");

var response = await client.SendWithDigestAuthAsync(request, HttpCompletionOption.ResponseContentRead, "username", "password");
```

## Compatibility

The `HttpClientDigestAuth` targets .NET Core 3.1, as this is most supported, being a LTS release, and stable .NET version before the newly released .NET 6.0.
