using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Forge.Museum.API.Controllers
{
    public class BaseApiController : ApiController
    {
        private NameValueCollection nvc;

        public BaseApiController()
        {

        }

        protected NameValueCollection NVC
        {
            get
            {
                if (nvc == null)
                {
                    nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                }

                return nvc;
            }
        }


        /// <summary>
        /// Parse specified URL parameter
        /// </summary>
        /// <param name="key">Url parameter to parse</param>
        /// <param name="defaultValue">A value to return if parameterd is missing or not parseable</param>
        /// <param name="errorOnNotFound">If true, throw 400 Bad request if parameter is missing</param>
        /// <returns>The parsed url parameter</returns>
        protected string GetUrlString(string key, string defaultValue = null, bool errorOnNotFound = false)
        {
            if (string.IsNullOrEmpty(NVC[key]))
            {
                if (errorOnNotFound)
                {
                    throw new HttpException(400, "Missing URL parameter: " + key);
                }
                return defaultValue;
            }
            else
            {
                return NVC[key];
            }
        }

        /// <summary>
        /// Parse specified URL parameter
        /// </summary>
        /// <param name="key">Url parameter to parse</param>
        /// <param name="defaultValue">A value to return if parameterd is missing or not parseable</param>
        /// <param name="errorOnNotFound">If true, throw 400 Bad request if parameter is missing</param>
        /// <returns>The parsed url parameter</returns>
        protected int? GetUrlInt(string key, int? defaultValue = null, bool errorOnNotFound = false)
        {
            if (string.IsNullOrEmpty(NVC[key]))
            {
                if (errorOnNotFound)
                {
                    throw new HttpException(400, "Missing URL parameter: " + key);
                }

                return defaultValue;
            }
            else
            {
                try
                {
                    if (int.Parse(NVC[key]) == -1)
                    {
                        return defaultValue;
                    }
                    else
                    {
                        return int.Parse(NVC[key]);
                    }
                }
                catch
                {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// Parse specified URL parameter
        /// </summary>
        /// <param name="key">Url parameter to parse</param>
        /// <param name="defaultValue">A value to return if parameterd is missing or not parseable</param>
        /// <param name="errorOnNotFound">If true, throw 400 Bad request if parameter is missing</param>
        /// <returns>The parsed url parameter</returns>
        protected bool? GetUrlBool(string key, bool? defaultValue = null, bool errorOnNotFound = false)
        {
            if (string.IsNullOrEmpty(NVC[key]))
            {
                if (errorOnNotFound)
                {
                    throw new HttpException(400, "Missing URL parameter: " + key);
                }

                return defaultValue;
            }
            else
            {
                try
                {
                    return bool.Parse(NVC[key]);
                }
                catch
                {
                    return defaultValue;
                }
            }
        }

        protected DateTime? GetUrlDate(string key, DateTime? defaultValue = null, bool errorOnNotFound = false)
        {
            if (string.IsNullOrEmpty(NVC[key]))
            {
                if (errorOnNotFound)
                {
                    throw new HttpException(400, "Missing URL parameter: " + key);
                }

                return defaultValue;
            }
            else
            {
                try
                {
                    return DateTime.Parse(NVC[key]);
                }
                catch
                {
                    return defaultValue;
                }
            }
        }

        protected string GetAuthHeaderValue()
        {
            var request = Request;
            var headers = request.Headers;

            if (headers.Contains("Authorization"))
            {
                return headers.Authorization.Parameter;
            }

            throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}