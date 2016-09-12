namespace Condominium2000.Helpers
{
	public class Constants
	{
		//************************
		//* FILES
		//************************
		public const string FileUploadPath = "~/Uploads/";
		public const string FileLinkPath = "/Uploads/";
		public const string FileFtpLink = "ftp://ftp.Condominium2000.se/Condominium2000.se/public_html";

		//************************
		//* GUI
		//************************
		public const string GuiAjaxLoaderPath = "/Content/Images/GIF/ajax-loader.gif";
		public const int GuiAjaxLoaderShowTime = 0;
		public const int GuiAjaxLoaderHideTime = 0;

		public const int GuiAjaxSuccesAndErrorShowTime = 3000;
		public const int GuiAjaxSuccesAndErrorHideTime = 0;
		public const int GuiAjaxSuccesAndErrorEase = 200;

		//************************
		//* REGEXP
		//************************
		public const string RegexpBreakForHtml = "(\\n(?!(\\s*(</p>|</b>|<li>|</li>|<ul>|</ul>|<ol>|</ol>))))";

		// To understand, get regexp tester and insert the following:
		// Sentence:    "\"[List]\n[*]Text\n[/ List] \""[link = "www.gp.se"]en länk
		// Regexp:      (\[(/){0,1}(\s)*(link|b|list|\*)((\s+)=(\s)".+")*\])
		public const string RegexpTranslatorTags =
			@"(\[(/){0,1}(\s)*(b|i|u||note|link|linktab|list|map|(list\s*=\s*1)|\*)((\s*)=(\s*)"".+"")*\])";

		//************************
		//* RECAPTCHA
		//************************
		public const string RecaptchaPublicKey = "6LczzdoSAAAAACRnjbfBDLmxqwzbQShRT7FbrsXV";
		public const string RecaptchaPrivateKey = "6LczzdoSAAAAANne7V0w_VpfaLkvJgnqrdVZF7BU";

		//************************
		//* MAIL
		//************************
		public const string MailErrorFormTo = "felanmalningar@Condominium2000.se";
		public const string MailBoardFormTo = "kontakt@Condominium2000.se";
		public const string MailItFormTo = "it@Condominium2000.se";

		//************************
		//* TRANSLATION
		//************************
		public const string TranslationUrlGoogleTranslate = "http://www.google.com/translate_t?hl=sv&ie=UTF8";

		//************************
		//* NEWS
		//************************
		/// <summary>
		///     DB
		/// </summary>
		public const int NewsDbTemplateId = 1;

		/// <summary>
		///     INDEX
		/// </summary>
		public const int NewsIndexNumberOfNews = 4;

		public const int NewsIndexTruncateSize = 600;

		/// <summary>
		///     SHOW ALL
		/// </summary>
		public const int NewsShowAllPaginateSize = 15;

		public const int NewsShowAllTruncateSize = 45;

		//************************
		//* SOCIETY
		//************************
		/// <summary>
		///     DB
		/// </summary>
		public const int SocietyDbTemplateId = 1;

		//************************
		//* RESIDENT
		//************************
		/// <summary>
		///     DB
		/// </summary>
		public const int ResidentDbTemplateId = 1;

		//************************
		//* CONTACT
		//************************
		/// <summary>
		///     DB
		/// </summary>
		public const int ContactDbTemplateId = 1;

		//************************
		//* ANNUALREPORT
		//************************
		/// <summary>
		///     CREATE
		/// </summary>
		public const string AnnualReportUploadPath = "AnnualReports/";

		//************************
		//* ANNUALREPORT
		//************************
		/// <summary>
		///     CREATE
		/// </summary>
		public const string AnnualMeetingUploadPath = "AnnualMeetings/";

		//************************
		//* GALLERYIMAGES
		//************************
		/// <summary>
		///     CREATE
		/// </summary>
		public const string GalleryImageUploadPath = "GalleryImages/";

		//************************
		//* DOCUMENTS
		//************************
		public const string DocumentUploadPath = "Documents/";

		//************************
		//* WEATHERINFORMATION
		//************************
		public const string WeatherInformationXmlData =
			"http://www.yr.no/sted/Sverige/V%C3%A4stra_G%C3%B6taland/G%C3%B6teborg/forecast.xml";

		public const int WeatherInformationMaxNrOfRead = 10;
		public const int WeatherInformationMaxPrecipitation = 3;

		//************************
		//* PREVIEW WINDOW
		//************************
		public const int PreviewWindowHeight = 600;
		public const int PreviewWindowWidth = 700;
	}
}