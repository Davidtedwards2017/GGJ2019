using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Text;
using Utilites;

namespace _ServerCom
{
    public abstract class ServerProvider<T>: Singleton<T> where T : MonoBehaviour
    {
        public ServerEndpointData ServerConfig;
        public int NumberOfServerAttempts = 5;

        protected IEnumerator Put(string address, string action, string content, string failMessage, Action OnFailure = null, Action OnSuccess = null)
        {
            var body = new JObject();
            if (content.Exists())
            {
                body.Add("body", content);
            }

            if (action.Exists())
            {
                body.Add("action", action);
            }

            yield return Put(address, body, failMessage, OnFailure, OnSuccess);
        }

        protected IEnumerator Get(string address, string failMessage,
                                 Action<UnityWebRequest> OnSuccess = null,
                                 Action<UnityWebRequest> OnFailure = null,
                                 Dictionary<string, string> customHeaders = null)
        {
            int attemptNumber = 0;
            while (NumberOfServerAttempts > attemptNumber++)
            {
                var request = Get(address);
                if (customHeaders != null)
                {
                    foreach (var entry in customHeaders)
                    {
                        request.SetRequestHeader(entry.Key, entry.Value);
                    }
                }

                yield return request.SendWebRequest();

                if (AssertServerResponse(request, failMessage, attemptNumber))
                {
                    if (OnSuccess != null)
                    {
                        OnSuccess.Invoke(request);
                    }
                    yield break;
                }

                if (OnFailure != null)
                {
                    OnFailure.Invoke(request);
                }

                if (request.IsClientErrorResponseCode()) //bad request, don't bother reattempting call
                {
                    break;
                }
            }
        }

        protected IEnumerator Put(string address, JObject body, string failMessage, Action OnFailure = null, Action OnSuccess = null)
        {
            int attemptNumber = 0;
            while (NumberOfServerAttempts > attemptNumber++)
            {
                var request = Put(address, body.ToString());

                yield return request.SendWebRequest();

                if (AssertServerResponse(request, failMessage, attemptNumber))
                {
                    if (OnSuccess != null)
                    {
                        OnSuccess.Invoke();
                    }
                    yield break;
                }
            }
            if (OnFailure != null)
            {
                OnFailure.Invoke();
            }
        }

        protected string BuildAddress(string collectionName, params string[] args)
        {
            var baseAddress = ServerConfig.EndPointAddress + "/" + collectionName + "/";
            baseAddress += string.Join("/", args);
            return baseAddress;
        }

        protected UnityWebRequest Get(string address)
        {
            var request = UnityWebRequest.Get(address);
            ServerAuthenticateRequest(ref request);
            return request;
        }

        protected UnityWebRequest Post(string address, WWWForm body)
        {
            var request = UnityWebRequest.Post(address, body);
            ServerAuthenticateRequest(ref request);
            return request;
        }

        protected UnityWebRequest Put(string address, string body)
        {
            var request = UnityWebRequest.Put(address, body);
            ServerAuthenticateRequest(ref request);
            request.SetRequestHeader("Content-Type", "application/json");
            return request;
        }

        protected UnityWebRequest Delete(string address)
        {
            var request = UnityWebRequest.Delete(address);
            ServerAuthenticateRequest(ref request);
            return request;
        }

        protected void ServerAuthenticateRequest(ref UnityWebRequest request)
        {
            string authHeader = BuildAuthenticationHeaderString(ServerConfig.ApiClientName, ServerConfig.ApiKeyValue);
            request.SetRequestHeader("AUTHORIZATION", authHeader);
        }

        protected string BuildAuthenticationHeaderString(string userName, string password)
        {
            string auth = userName + ":" + password;
            auth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
            auth = "Basic " + auth;
            return auth;
        }


        protected bool AssertServerResponse(UnityWebRequest response, string errorMsg, int attemptNumber, params long[] expectedResonseCodes)
        {
            if (!response.IsSuccessfullResponseCode(expectedResonseCodes) || response.isNetworkError)
            {
                Debug.LogError(string.Format("{0}:\n ResponseCode {1}: '{2}' --- attempt ({3}/{4})",
                        errorMsg, response.responseCode, response.error, attemptNumber, NumberOfServerAttempts));
                return false;
            }
            return true;
        }

    }
}