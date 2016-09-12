using System;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Condominium2000.Helpers;
using Condominium2000.Helpers.Session;
using Condominium2000.Models;
using Condominium2000.Models.Context;
using Condominium2000.Models.ViewModels;

namespace Condominium2000.Controllers
{
	public class InformationPanelController : Controller
	{
		private Condominium2000Context _db = new Condominium2000Context();

		//
		// GET: /InformationPanel/

		public ActionResult PanelNews()
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.News;

			/**
			 * Fill Model
			 */
			// Index should have limited amount of news
			var viewModel = new NewsViewModel(ref _db, NewsViewModel.DisplayNews.Limited);

			/**
			 * Return View
			 */
			return View(viewModel);
		}

		//
		// GET: /InformationPanel/ShowNews/5

		public ActionResult ShowNews(int id)
		{
			/**
			 * Set Menu in Session Object
			 */
			SessionHelper.SessionMenu = SessionHelper.Menu.News;

			/**
			 * Fill Model
			 */
			// Index should have limited amount of news
			var viewModel = new NewsViewModel(ref _db, NewsViewModel.DisplayNews.All);
			// Get the selected news
			viewModel.SelectedNews = viewModel.News.FirstOrDefault(n => n.Id == id);

			/**
			 * Return View
			 */
			return View(viewModel);
		}


		private WeatherInformation ParseWeatherInformation(XmlReader reader)
		{
			var wi = new WeatherInformation();

			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					try
					{
						switch (reader.Name)
						{
							case "time":
								reader.MoveToNextAttribute();
								wi.From = reader.ReadContentAsDateTime();
								reader.MoveToNextAttribute();
								wi.To = reader.ReadContentAsDateTime();
								break;

							case "symbol":
								reader.MoveToNextAttribute();
								reader.MoveToNextAttribute();
								reader.MoveToNextAttribute();
								wi.WeatherSymbol = reader.ReadContentAsString();
								break;

							case "temperature":
								reader.MoveToNextAttribute();
								reader.MoveToNextAttribute();
								wi.Temperature = reader.ReadContentAsInt();
								break;

							case "windDirection":
								reader.MoveToNextAttribute();
								wi.WindDegree = reader.ReadContentAsDouble();
								break;

							case "windSpeed":
								reader.MoveToNextAttribute();
								wi.WindSpeed = reader.ReadContentAsDouble();
								reader.MoveToNextAttribute();
								wi.WindDirection = reader.ReadContentAsString();

								// Calculate the Wind symbol (if possible)
								if (wi.WindDegree != null)
								{
									wi.WindSymbol = string.Format("{0}.{1}"
										, ((int) Math.Round(wi.WindSpeed*10/25)*25).ToString("D4")
										, Math.Round((double) wi.WindDegree/5)*5);
								}

								break;

							case "precipitation":
								reader.MoveToNextAttribute();

								var validAttribute = true;
								var maxWhile = 0;
								while (validAttribute && (maxWhile < Constants.WeatherInformationMaxPrecipitation))
								{
									switch (reader.Name)
									{
										case "value":
											wi.WeatherPrecipitation.Value = reader.ReadContentAsDouble();
											reader.MoveToNextAttribute();
											break;

										case "minvalue":
											wi.WeatherPrecipitation.MinValue = reader.ReadContentAsDouble();
											if (wi.WeatherPrecipitation.MaxValue != null)
											{
												wi.WeatherPrecipitation.TwoValue = true;
											}
											reader.MoveToNextAttribute();
											break;

										case "maxvalue":
											wi.WeatherPrecipitation.MaxValue = reader.ReadContentAsDouble();
											if (wi.WeatherPrecipitation.MinValue != null)
											{
												wi.WeatherPrecipitation.TwoValue = true;
											}
											reader.MoveToNextAttribute();
											break;

										default:
											validAttribute = false;
											break;
									}
									maxWhile++;
								}
								break;
						}
					}
					catch (Exception)
					{
						// Continue?
					}
				}
			}


			return wi;
		}

		public ActionResult Weather()
		{
			var weather = new Weather();

			var xmlWeather = XDocument.Load(Constants.WeatherInformationXmlData);

			// Get next update
			weather.NextUpdate =
				DateTime.Parse(xmlWeather.Element("weatherdata")?.Element("meta")?.Element("nextupdate")?.Value).AddSeconds(30);

			// Get the weather information
			var xmlTabular = xmlWeather.Element("weatherdata")?.Element("forecast")?.Element("tabular");
			var reader = xmlTabular?.CreateReader();

			const int numberOfRead = 0;
			while (reader != null && reader.Read())
			{
				if ((reader.NodeType == XmlNodeType.Element)
					&& (reader.Name == "time")
					&& (numberOfRead <= Constants.WeatherInformationMaxNrOfRead))
				{
					var wi = ParseWeatherInformation(reader.ReadSubtree());
					if (wi != null)
					{
						if (wi.From.Day == DateTime.Today.Day)
						{
							weather.Today.Add(wi);
						}
						else if (wi.From.Day == DateTime.Today.Day + 1)
						{
							weather.Tomorrow.Add(wi);
						}
						else if (wi.From.Day == DateTime.Today.Day + 2)
						{
							weather.AfterTomorrow.Add(wi);
						}
					}
				}
			}

			return View(weather);
		}
	}
}