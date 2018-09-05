using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace Battleships.Tests
{
	internal static class ActionResultExtensions
	{
		public static ObjectResult AssertStatus(this ObjectResult result, HttpStatusCode code)
		{
			result.StatusCode.Should().Be((int) code);
			return result;
		}

		public static void AssertStatus(this StatusCodeResult result, HttpStatusCode code)
		{
			result.StatusCode.Should().Be((int)code);
		}

		public static void AssertOkResult(this IActionResult result)
		{
			result.Should().NotBeNull();

			var okResult = result as OkResult;

			okResult.Should().NotBeNull();

			okResult.AssertStatus(HttpStatusCode.OK);
		}

		public static ObjectResult AssertOkObjectResult(this IActionResult result)
		{
			result.Should().NotBeNull();

			var okObjectResult = result as OkObjectResult;

			okObjectResult.Should().NotBeNull();
			okObjectResult.AssertStatus(HttpStatusCode.OK);

			return okObjectResult;
		}

		public static T Model<T>(this ObjectResult result)
		{
			return (T)result.Value;
		}
	}
}
