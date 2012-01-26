using System;

namespace GoogleAddressVerify
{
	class Program
	{
		static void Main(string[] args)
		{
			log4net.Config.XmlConfigurator.Configure();


			var query = Console.ReadLine();
			while (query != "/quit")
			{
				var address = StandardizeAddress.Standardize(query);
				
				if(address.IsValid)
					Console.WriteLine(address.FormattedAddress);
				else
					Console.WriteLine("That does not appear to be a valid address");

				query = Console.ReadLine();
			}
		}
	}
}
