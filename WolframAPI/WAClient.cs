

using WolframAPI.Exceptions;

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
        /// Gets the result of the specified expression.
        /// <para>The expression is returned as <see cref="WAResult"/>WAResult</para> 
        /// so you can manually go through the pods of the response (to get ANY information you'd like)
        /// <para>It is encouraged to use this method instead of <see cref="Solve"/>Solve</para>
        /// </summary>
        /// <param name="expression">The expression to solve.</param>
        /// <exception cref="WolframException">Throws in case of any error.</exception>
        /// <returns>The result</returns>
        public WAResult GetResult(string expression)
        {
            var response = Submit(expression);
            var result = Parse(response);

            return result;
        }

        /// <summary>
        /// Submits the specified expression and returns the raw result.
        /// </summary>
        /// <param name="expression">The expression to post.</param>
        /// <exception cref="WolframException">Throws in case of any error.</exception>
        /// <returns>Raw response</returns>
        public string Submit(string expression)
        {
            try
            {
                expression = expression.Replace("=", " = ");
                var wacode = HttpUtility.UrlEncode(expression);

                var url = new Uri(string.Format(BaseUrl, _appId, wacode));

                string returned;

                using (var client = new WebClient())
                {
                    returned = client.DownloadString(url);
                }

                return returned;
            }
            catch(WebException x)
            {
                throw new WolframException("WebException thrown while getting the response.", x);
            }
            catch(Exception x)
            {
                throw new WolframException("Unhandled exception thrown while submitting the expression.", x);
            }
        }

        /// <summary>
        /// Parses the raw response.
        /// </summary>
        /// <param name="response">The response to parse</param>
        /// <returns>The parsed response</returns>
        /// <exception cref="WolframException">Throws in case of any error.</exception>
        public WAResult Parse(string response)
        {
            try
            {
                var serializer = new XmlSerializer(typeof (WAResult));

                WAResult result;

                using (var reader = new StringReader(response))
                {
                    result = serializer.Deserialize(reader) as WAResult;
                }

                return result;
            }
            catch(ArgumentNullException x)
            {
                throw new WolframException("Argument null exception thrown. The response passed to Parse() was probably empty.", x);
            }
            catch(InvalidOperationException x)
            {
                throw new WolframException("Exception thrown while deserializing the response.", x);
            }
        }
    }
}
