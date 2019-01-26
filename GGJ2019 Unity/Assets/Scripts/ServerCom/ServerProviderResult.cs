using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace _ServerCom
{
    public enum ProviderResultStatus
    {
        Success,
        Fail,
        Missing,
    }

    public class ServerProviderResult<T>
    {
        public ProviderResultStatus Status;
        public string ResultMessage;
        public T Content;

        public static ServerProviderResult<T> Failed(string message)
        {
            return new ServerProviderResult<T>
            {
                Status = ProviderResultStatus.Fail,
                ResultMessage = message
            };
        }

        public static ServerProviderResult<T> Success(T content)
        {
            return new ServerProviderResult<T>
            {
                Status = ProviderResultStatus.Success,
                Content = content
            };
        }

        public static ServerProviderResult<T> Missing(T content)
        {
            return new ServerProviderResult<T>
            {
                Status = ProviderResultStatus.Missing,
                Content = content
            };
        }
    }
}