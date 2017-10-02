using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace KrisHemenway.CommonCore
{
	public class JsonContent : StringContent
	{
		public JsonContent(object objectToSerialize) : base(JsonConvert.SerializeObject(objectToSerialize), Encoding.UTF8, "application/json") { }
	}
}
