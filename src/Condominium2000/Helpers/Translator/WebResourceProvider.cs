using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Condominium2000.Helpers.Translator
{
	public abstract class WebResourceProvider
	{
		/// <summary>HTTP status code.</summary>
		private HttpStatusCode _mHttpStatusCode;

		///////////////////////////
		// Implementation (members)

		/// <summary>User agent string used when making an HTTP request.</summary>
		private string _mStrAgent;

		/// <summary>Referer string used when making an HTTP request.</summary>
		private string _mStrReferer;

		/// <summary>
		///     Default constructor.
		/// </summary>
		protected WebResourceProvider()
		{
			Reset();
		}

		/////////////
		// Properties

		/// <summary>
		///     Gets and sets the user agent string.
		/// </summary>
		public string Agent
		{
			get { return _mStrAgent; }
			set { _mStrAgent = value ?? ""; }
		}

		/// <summary>
		///     Gets and sets the referer string.
		/// </summary>
		public string Referer
		{
			get { return _mStrReferer; }
			set { _mStrReferer = value ?? ""; }
		}

		/// <summary>
		///     Gets and sets the minimum pause time interval (in mSec).
		/// </summary>
		public int Pause { get; set; }

		/// <summary>
		///     Gets and sets the timeout (in mSec).
		/// </summary>
		public int Timeout { get; set; }

		/// <summary>
		///     Returns the retrieved content.
		/// </summary>
		public string Content { get; private set; }

		/// <summary>
		///     Gets the fetch timestamp.
		/// </summary>
		public DateTime FetchTime { get; private set; }

		/// <summary>
		///     Gets the last error message, if any.
		/// </summary>
		public string ErrorMsg { get; private set; }

		/////////////
		// Operations

		/// <summary>
		///     Resets the state of the object.
		/// </summary>
		public void Reset()
		{
			_mStrAgent = "Mozilla/4.0 (compatible; MSIE 5.5; Windows NT 5.0)";
			_mStrReferer = "";
			ErrorMsg = "";
			Content = "";
			_mHttpStatusCode = HttpStatusCode.OK;
			Pause = 0;
			Timeout = 0;
			FetchTime = DateTime.MinValue;
		}

		/// <summary>
		///     Fetches the web resource.
		/// </summary>
		public void FetchResource()
		{
			// Initialize the provider - quit if initialization fails
			if (!Init())
				return;

			// Main loop
			bool bOk;
			do
			{
				var url = GetFetchUrl();
				GetContent(url);
				bOk = _mHttpStatusCode == HttpStatusCode.OK;
				if (bOk)
					ParseContent();
			} while (bOk && ContinueFetching());
		}

		//////////////////
		// Virtual methods

		/// <summary>
		///     Provides the derived class with an opportunity to initialize itself.
		/// </summary>
		/// <returns>true if the operation succeeded, false otherwise.</returns>
		protected virtual bool Init()
		{
			return true;
		}

		/// <summary>
		///     Returns the url to be fetched.
		/// </summary>
		/// <returns>The url to be fetched.</returns>
		protected abstract string GetFetchUrl();

		/// <summary>
		///     Retrieves the POST data (if any) to be sent to the url to be fetched.
		///     The data is returned as a string of the form &quot;arg=val [&amp;arg=val]...&quot;.
		/// </summary>
		/// <returns>A string containing the POST data or null if none.</returns>
		protected virtual string GetPostData()
		{
			return null;
		}

		/// <summary>
		///     Provides the derived class with an opportunity to parse the fetched content.
		/// </summary>
		protected virtual void ParseContent()
		{
		}

		/// <summary>
		///     Informs the framework that it needs to continue fetching urls.
		/// </summary>
		/// <returns>
		///     true if the framework needs to continue fetching urls, false otherwise.
		/// </returns>
		protected virtual bool ContinueFetching()
		{
			return false;
		}

		///////////////////////////
		// Implementation (methods)

		/// <summary>
		///     Retrieves the content of the url to be fetched.
		/// </summary>
		/// <param name="url">Url to be fetched.</param>
		private void GetContent
			(string url)
		{
			// Pause, if necessary
			if (Pause > 0)
			{
				var nElapsedMsec = 0;
				do
				{
					// Determine the time elapsed since the last fetch (if any)
					if (nElapsedMsec == 0)
					{
						if (FetchTime != DateTime.MinValue)
						{
							var tsElapsed = FetchTime - DateTime.Now;
							nElapsedMsec = (int)tsElapsed.TotalMilliseconds;
						}
					}

					// Pause 100mSec increment if necessary
					var nSleepMsec = 100;
					if (nElapsedMsec < Pause)
					{
						Thread.Sleep(nSleepMsec);
						nElapsedMsec += nSleepMsec;
					}
				} while (nElapsedMsec < Pause);
			}

			// Set up the fetch request
			var strUrl = url;
			if (!strUrl.StartsWith("http://"))
				strUrl = "http://" + strUrl;
			var req = (HttpWebRequest)WebRequest.Create(strUrl);
			req.AllowAutoRedirect = true;
			req.UserAgent = _mStrAgent;
			req.Referer = _mStrReferer;
			if (Timeout != 0)
				req.Timeout = Timeout;

			// Add POST data (if present)
			var strPostData = GetPostData();
			if (strPostData != null)
			{
				var asciiEncoding = new ASCIIEncoding();
				var postData = asciiEncoding.GetBytes(strPostData);
				req.Method = "POST";
				req.ContentType = "application/x-www-form-urlencoded";
				req.ContentLength = postData.Length;

				var reqStream = req.GetRequestStream();
				reqStream.Write(postData, 0, postData.Length);
				reqStream.Close();
			}

			// Fetch the url - return on error
			ErrorMsg = "";
			Content = "";
			HttpWebResponse resp = null;
			try
			{
				FetchTime = DateTime.Now;
				resp = (HttpWebResponse)req.GetResponse();
			}
			catch (Exception exc)
			{
				if (exc is WebException)
				{
					var webExc = exc as WebException;
					ErrorMsg = webExc.Message;
				}
				return;
			}
			finally
			{
				if (resp != null)
					_mHttpStatusCode = resp.StatusCode;
			}

			// Store retrieved content
			try
			{
				var stream = resp.GetResponseStream();
				var streamReader = new StreamReader(stream);
				Content = streamReader.ReadToEnd();
			}
			catch (Exception)
			{
				// Read failure occured - nothing to do
			}
		}
	}
}