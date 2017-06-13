using System;
using System.IO;
using System.Net;
using System.Text;
using OAuth.Net.Common;
using OAuth.Net.Components;

namespace ConsoleApplication2
{

        public class ServiceProvider
        {
            private const int RequestTimeOut = 1000 * 60 * 10;
            private readonly string _consumerKey;
            private readonly string _consumerSecret;
            private readonly string _signType;
            private readonly Uri _serviceProviderUri;
            private static readonly ISigningProvider SigningProvider = new HmacSha1SigningProvider();
            private static readonly INonceProvider NonceProvider = new GuidNonceProvider();
            private string[] validSignType = { "HMAC-SHA1", "PLAINTEXT" };

            public ServiceProvider(string endpoint, string consumerKey, string consumerSecret, string signType)
            {
                _consumerKey = consumerKey;
                _consumerSecret = consumerSecret;
                _serviceProviderUri = new Uri(endpoint);
                
                // check if sign is valid
                var isValidSign = Array.Exists(validSignType, element => element == signType);

                if (isValidSign == false) 
                    throw new Exception(string.Format("Error, the signType must be either HMAC-SHA1 or PLAINTEXT"));

                _signType = signType;

            }

            private HttpWebRequest GenerateRequest(string contentType, string requestMethod)
            {
                var ts = UnixTime.ToUnixTime(DateTime.Now);
                var param = new OAuthParameters()
                {
                    ConsumerKey = _consumerKey,
                    SignatureMethod = _signType,
                    Version = Constants.Version1_0,
                    Nonce = NonceProvider.GenerateNonce(ts),
                    Timestamp = ts.ToString(),
                };

                //Generate Signature Hash
                // var signatureBase = SignatureBase.Create(requestMethod.ToUpper(), _serviceProviderUri, param);
                //Set Signature Hash as one of the OAuth Parameter
                //param.Signature = SigningProvider.ComputeSignature(signatureBase, _consumerSecret, null);

                if (_signType == "HMAC-SHA1")
                {
                    var signatureBase = SignatureBase.Create(requestMethod.ToUpper(), _serviceProviderUri, param);
                    //Set Signature Hash as one of the OAuth Parameter
                    param.Signature = SigningProvider.ComputeSignature(signatureBase, _consumerSecret, null);
                }

                if (_signType == "PLAINTEXT")
                {
                    param.Signature = _consumerSecret + "&";
                }
        
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(_serviceProviderUri);
                httpWebRequest.Method = requestMethod;
                httpWebRequest.ContentType = contentType;
                //httpWebRequest.Timeout = RequestTimeOut;
                //Add the OAuth Parameters to Authorization Header of Request
                httpWebRequest.Headers.Add(Constants.AuthorizationHeaderParameter, param.ToHeaderFormat());
                return httpWebRequest;
            }

            public string GetData(string contentType)
            {
                var request = GenerateRequest(contentType, WebRequestMethods.Http.Get);
                return GetRequestResponse(request);
            }

            public string PostData(string contentType, string data)
            {
                var request = GenerateRequest(contentType, WebRequestMethods.Http.Post);
                var bytes = Encoding.ASCII.GetBytes(data);
                request.ContentLength = bytes.Length;
                if (bytes.Length > 0)
                {
                    using (var requestStream = request.GetRequestStream())
                    {
                        if (!requestStream.CanWrite) throw new Exception("The data cannot be written to request stream");
                        try
                        {
                            requestStream.Write(bytes, 0, bytes.Length);
                        }
                        catch (Exception exception)
                        {
                            throw new Exception(string.Format("Error while writing data to request stream - {0}", exception.Message));
                        }
                    }
                }
                return GetRequestResponse(request);
            }

        public string PutData(string contentType, string data)
        {
            var request = GenerateRequest(contentType, "PUT");
            var bytes = Encoding.ASCII.GetBytes(data);
            request.ContentLength = bytes.Length;

            if (bytes.Length > 0)
            {
                using (var requestStream = request.GetRequestStream())
                {
                    if (!requestStream.CanWrite) throw new Exception("The data cannot be written to request stream");
                    try
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                    }
                    catch (Exception exception)
                    {
                        throw new Exception(string.Format("Error while writing data to request stream - {0}", exception.Message));
                    }
                }
            }
            return GetRequestResponse(request);
        }

        public string DeleteData(string contentType, string data)
            {
                var request = GenerateRequest(contentType, "DELETE");
                var bytes = Encoding.ASCII.GetBytes(data);
                request.ContentLength = bytes.Length;

                if (bytes.Length > 0)
                {
                    using (var requestStream = request.GetRequestStream())
                    {
                        if (!requestStream.CanWrite) throw new Exception("The data cannot be written to request stream");
                        try
                        {
                            requestStream.Write(bytes, 0, bytes.Length);
                        }
                        catch (Exception exception)
                        {
                            throw new Exception(string.Format("Error while writing data to request stream - {0}", exception.Message));
                        }
                    }
                }
                return GetRequestResponse(request);
            }

            private static string GetRequestResponse(HttpWebRequest httpWebRequest)
            {
                if (httpWebRequest == null) throw new ArgumentNullException("httpWebRequest");
                string responseString = null;
                try
                {
                    using (var response = (HttpWebResponse) httpWebRequest.GetResponse())
                    {
                        using (var responseStream = response.GetResponseStream())
                        {
                            if (responseStream != null)
                            {
                                var reader = new StreamReader(responseStream);
                                responseString = reader.ReadToEnd();
                                reader.Close();
                                responseStream.Close();
                            }
                        }
                        response.Close();
                    }
                }
                catch (WebException webException)
                {
                    throw new Exception(string.Format("WebException while reading response - {0}", webException.Message));
                }
                catch (Exception exception)
                {
                    throw new Exception(string.Format("Unhandled exception while reading response - {0}", exception.Message));
                }
                return responseString;
            }

        }
    }


