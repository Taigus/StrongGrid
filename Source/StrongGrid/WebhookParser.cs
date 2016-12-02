﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using StrongGrid.Model.Webhooks;
using StrongGrid.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace StrongGrid
{
	/// <summary>
	/// Allows parsing of information posted from SendGrid.
	/// This parser supports both 'Events' and 'Inbound emails'.
	/// </summary>
	public class WebhookParser
	{
		#region FIELDS

		#endregion

		#region PROPERTIES

		#endregion

		#region CTOR

		/// <summary>
		/// Initializes a new instance of the <see cref="WebhookParser" /> class.
		/// </summary>
		public WebhookParser()
		{
		}

		#endregion

		#region PUBLIC METHODS

		/// <summary>
		/// Parses the webhook events asynchronously.
		/// </summary>
		/// <param name="httpContext">The HTTP context.</param>
		/// <returns>An array of <see cref="Event">events</see>.</returns>
		public Task<Event[]> ParseWebhookEventsAsync(HttpContext httpContext)
		{
			return ParseWebhookEventsAsync(httpContext.Request);
		}

		/// <summary>
		/// Parses the webhook events asynchronously.
		/// </summary>
		/// <param name="httpRequest">The HTTP request.</param>
		/// <returns>An array of <see cref="Event">events</see>.</returns>
		public Task<Event[]> ParseWebhookEventsAsync(HttpRequest httpRequest)
		{
			return ParseWebhookEventsAsync(httpRequest.Body);
		}

		/// <summary>
		/// Parses the webhook events asynchronously.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns>An array of <see cref="Event">events</see>.</returns>
		public async Task<Event[]> ParseWebhookEventsAsync(Stream stream)
		{
			string requestBody;
			using (var streamReader = new StreamReader(stream))
			{
				requestBody = await streamReader.ReadToEndAsync().ConfigureAwait(false);
			}

			var webHookEvents = JsonConvert.DeserializeObject<List<Event>>(
				requestBody,
				new WebHookEventConverter());

			return webHookEvents.ToArray();
		}

		#endregion
	}
}