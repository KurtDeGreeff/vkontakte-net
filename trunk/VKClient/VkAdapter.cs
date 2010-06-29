using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml.Linq;
using Vkontakte.Exceptions;
using Vkontakte.MethodResults;

namespace Vkontakte
{
    public class VkAdapter:IVkAdapter
    {
        private SessionData SessionData;

        public int UserId { get; set; }
        public int AppId { get; set; }

        public bool Authenticated { get; set; }

        public VkAdapter(SessionData sessionData, int userId, int appId)
        {
            Authenticated = false;
            Authenticate(sessionData, userId, appId);
        }

        public bool Authenticate(SessionData sessionData, int userId, int appId)
        {
            if(sessionData != null & userId > 0 & appId > 0)
            {
                Authenticated = true;
                SessionData = sessionData;
                UserId = userId;
                AppId = appId;
                return true;
            }
            else
            {
                Authenticated = false;
                return false;
            }
        }

        // TODO: Check session expiration time before calling the method
        public T CallRemoteMethod<T>(string name, string version, Func<XElement, T> resultMethod, Dictionary<String, String> methodParams = null)
        {
            if(!Authenticated)
            {
                throw new SecurityException("User is not authenticated. Please, call Authenticate() method first.");
            }
            methodParams = methodParams ?? new Dictionary<string, string>();

            string sig = Utils.MakeMethodSig(this.UserId, this.AppId, name, this.SessionData.SessionId, this.SessionData.SecretKey, methodParams);
            string url = Utils.GetRequestUrl(sig, this.SessionData.SessionId, methodParams);
            XElement root = XElement.Load(url);
            var result = ParseMethodResults<T>(root, resultMethod);
            return result;
        }

        public void CallRemoteMethodAsync()
        {
            throw new NotImplementedException();
        }

        protected T ParseMethodResults<T>(XElement response, Func<XElement, T> parseMethod)
        {
            T result;

            if (response.Name.LocalName == "error")
            {
                var errorResult = ParseErrorResult(response); 
                throw new RemoteMethodException(errorResult.ErrorCode, errorResult.ErrorMessage);
            }

            try
            {
                result = parseMethod(response);
                return result;
            }
            catch (System.NullReferenceException ex)
            {
                throw new RemoteMethodException(ErrorCode.ParsingOfResultFailed, Messages.ErrorMessages[ErrorCode.ParsingOfResultFailed]);
            }
        }

        protected ErrorResult ParseErrorResult(XElement error)
        {
            var errorCode = new ErrorCode();
            var errorMessage = "";

            try
            {
                Enum.TryParse(error.Element("error_code").Value, out errorCode);
            }
            catch (NullReferenceException)
            {
                errorCode = ErrorCode.UnknownErrorOccured;
            }

            try
            {
                errorMessage = error.Element("error_msg").Value;
            }
            catch (NullReferenceException)
            {
                errorMessage = "Unknown error.";
            }


            var result = new ErrorResult() { ErrorCode = errorCode, ErrorMessage = errorMessage };
            return result;
        }
    }
}
