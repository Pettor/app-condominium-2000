using System;
using System.Collections.Generic;

namespace Condominium2000.Models
{
	public class Weather
	{
		public Weather()
		{
			Today = new List<WeatherInformation>();
			Tomorrow = new List<WeatherInformation>();
			AfterTomorrow = new List<WeatherInformation>();
		}

		public string LocationName { get; set; }
		public string CountryName { get; set; }
		public List<WeatherInformation> Today { get; set; }
		public List<WeatherInformation> Tomorrow { get; set; }
		public List<WeatherInformation> AfterTomorrow { get; set; }
		public DateTime NextUpdate { get; set; }
	}

	public enum WeatherSymbol
	{
		Symbol1 = 1,
		Snow = 2,
		PartlyCloudy = 3,
		Cloudy = 4,
		Symbol5 = 5,
		Symbol6 = 6,
		Symbol7 = 7,
		Symbol8 = 8,
		Rain = 9,
		StrongRain = 10,
		Snow2 = 13
	}

	public class WeatherInformation
	{
		public WeatherInformation()
		{
			WeatherPrecipitation = new WeatherPrecipitation();
			WindDegree = null;
		}

		public string WeatherSymbol { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public int Temperature { get; set; }
		public double WindSpeed { get; set; }
		public double? WindDegree { get; set; }
		public string WindSymbol { get; set; }
		public string WindDirection { get; set; }
		public WeatherPrecipitation WeatherPrecipitation { get; set; }
	}

	public class WeatherPrecipitation
	{
		public WeatherPrecipitation()
		{
			TwoValue = false;
			MinValue = null;
			MaxValue = null;
		}

		public bool TwoValue { get; set; }
		public double Value { get; set; }
		public double? MinValue { get; set; }
		public double? MaxValue { get; set; }
	}
}