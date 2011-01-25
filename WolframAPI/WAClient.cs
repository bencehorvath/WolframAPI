

namespace WolframAPI
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Xml.Serialization;

    /// <summary>
    /// Used to access Wolfram Alpha. 
    /// Submits expressions, retrieves and parses responses.
    /// </summary>
    public sealed class WAClient
    {

        /// <summary>
        /// The base WA API url.
        /// </summary>
        public const string BaseUrl =
            "http://api.wolframalpha.com/v2/query?appid={0}&input={1}&format=image,plaintext";

        /// <summary>
        /// The application ID.
        /// </summary>
        private readonly string _appId;

        /// <summary>
        /// Initializes a new instance of the <see cref="WAClient"/> class.
        /// </summary>
        /// <param name="appId">The application ID provided by Wolfram. You have to request one for each of your apps.</param>
        public WAClient(string appId)
        {
            _appId = appId;
        }

        /// <summary>
        /// Solves the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The solution of the given expression</returns>
        public string Solve(string expression)
        {
            var response = Submit(expression);
            var result = Parse(response);

            var solution = from pod in result.Pods
                           where pod.Title.ToLower().Contains("solution")
                           select pod;

            return solution.Count() <= 0 ? "No solution." : solution.First().SubPods[0].PlainText;
        }

        /// <summary>
        /// Submits the specified expression and returns the raw result.
        /// </summary>
        /// <param name="expression">The expression to post.</param>
        /// <returns>Raw response</returns>
        public string Submit(string expression)
        {
            expression = expression.Replace("=", " = ");
            var wacode = HttpUtility.UrlEncode(expression);

            var url = new Uri(string.Format(BaseUrl, _appId, wacode));

            string returned;

            using(var client = new WebClient())
            {
                returned = client.DownloadString(url);
            }

            return returned;
        }

        /// <summary>
        /// Parses the raw response.
        /// </summary>
        /// <param name="response">The response to parse</param>
        /// <returns>The parsed response</returns>
        public WAResult Parse(string response)
        {
            var serializer = new XmlSerializer(typeof (WAResult));

            WAResult result;

            using(var reader = new StringReader(response))
            {
                result = serializer.Deserialize(reader) as WAResult;
            }

            return result;
        }
    }
}
