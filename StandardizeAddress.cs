using System;
using log4net;

namespace GoogleAddressVerify
{
	public class StandardizeAddress
	{
		private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public static Address Standardize(string searchQuery)
		{
			const string url = "http://maps.googleapis.com/maps/api/geocode/json?sensor=true&address=";

			dynamic googleResults = new Uri(url + searchQuery).GetDynamicJsonObject();

			var address = new Address();

			try
			{
				foreach (var result in googleResults.results)
				{
					Log.DebugFormat("Raw result: {0}", result);

					address.Latitude = result.geometry.location.lat;
					address.Longitude = result.geometry.location.lng;
					address.FormattedAddress = result.formatted_address;

					Log.DebugFormat("[{0},{1}] {2}", result.geometry.location.lat, result.geometry.location.lng, result.formatted_address);

					foreach (var item in result.address_components)
					{
						if (item.types[0] == "route")
						{
							address.StreetAddress = item.short_name.ToString();
						}
						if (item.types[0] == "locality")
						{
							address.City = item.short_name.ToString();
						}
						if (item.types[0] == "country")
						{
							address.Country = item.short_name.ToString();
						}
						if (item.types[0] == "administrative_area_level_1" || item.types[0] == "administrative_area_level_2" || item.types[0] == "administrative_area_level_3")
						{
							address.State = item.short_name.ToString();
						}
						if (item.types[0] == "street_number")
						{
							address.StreetNumber = item.short_name.ToString();
						}
						if (item.types[0] == "postal_code")
						{
							address.PostalCode = item.short_name.ToString();
						}
					}
					Log.DebugFormat("Formatted address: {0}", address.FormattedAddress);
				}
				address.IsValid = true;
			}
			catch (Exception)
			{
				address.IsValid = false;
			}

			return address;
		}
	}
}
