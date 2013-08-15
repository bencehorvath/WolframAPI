//
//  Program.cs
//
//  Author:
//       Bence Horváth <horvathb@me.com>
//
//  Copyright (c) 2013 Bence Horváth
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Diagnostics;

namespace WolframAPI.ConsoleTool
{
	class MainClass
	{
		const string WolframAppId = "557QYQ-UUUWTKX95V";

		public static void Main (string[] args)
		{

			Console.WriteLine ("Welcome to WolframAPI console.");
			Console.WriteLine ("This is a test console for the API, please type in an operation to receive results.");
			Console.WriteLine ("[DEBUG]: Using Wolfram AppID: {0}{1}", WolframAppId, Environment.NewLine);

			string operation = string.Empty;
			var client = new WAClient (WolframAppId);

			while (!operation.Equals("stop", StringComparison.InvariantCultureIgnoreCase)) {

				Console.Write ("> ");
				operation = Console.ReadLine ();

				if (operation.Equals ("stop", StringComparison.InvariantCulture))
					break;

				Console.WriteLine ("Answer: {0}", client.Solve (operation));

			}



		}
	}
}
