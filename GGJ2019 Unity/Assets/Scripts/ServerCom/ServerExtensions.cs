using System.Linq;
using UnityEngine.Networking;

public static class ServerExtensions
{

    public static bool IsSuccessfullResponseCode(this UnityWebRequest request)
    {
        switch (request.responseCode)
        {
            case 200:
            case 201:
            case 202:
            case 203:
            case 204:
            case 205:
            case 206:
            case 207:
            case 208:
            case 226:
                return true;
            default:
                return false;
        }
    }

    public static bool IsClientErrorResponseCode(this UnityWebRequest request)
    {
        switch (request.responseCode)
        {
            case 400:
            case 401:
            case 402:
            case 403:
            case 404:
            case 405:
            case 406:
            case 407:
            //case 408:
            case 409:
                return true;
            default:
                return false;
        }
    }

    public static bool IsSuccessfullResponseCode(this UnityWebRequest request, params long[] expectedResponseCodes)
    {
        if (expectedResponseCodes == null || !expectedResponseCodes.Any())
        {
            return request.IsSuccessfullResponseCode();
        }

        return expectedResponseCodes.Contains(request.responseCode);
    }

}
