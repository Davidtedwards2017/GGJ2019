using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Server Configuration", menuName = "ServerCom/EndpointConfig")]
public class ServerEndpointData : ScriptableObject
{
    public enum Server
    {
        Local,
        Development,
        Staging,
        Production
    }

    public Server ServerType;

    public string EndPointAddress
    {
        get
        {
            switch (ServerType)
            {
                case Server.Local:
                    return LocalEndPoint;
                case Server.Development:
                    return DevelopmentEndPoint;
                case Server.Staging:
                    return StagingEndPoint;
                case Server.Production:
                    return ProductionEndPoint;
            }
            return "";
        }
    }

    public string LocalEndPoint;
    public string DevelopmentEndPoint;
    public string StagingEndPoint;
    public string ProductionEndPoint;

    public string ApiClientName;
    public string ApiKeyValue;
}