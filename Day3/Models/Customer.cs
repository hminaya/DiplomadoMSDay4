using System;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace Day3.Models
{
	[JsonObject(Title = "customers")]
	public class Customer : BindableBase
	{

		[JsonProperty(PropertyName ="id")]
		public string id { get; set; }

		[JsonProperty(PropertyName ="name")]
		public string name { get; set; }

		[JsonProperty(PropertyName ="email")]
		public string email { get; set; }


	}
}
