using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml.Linq;
using Vkontakte.Constants;
using Vkontakte.Exceptions;
using Vkontakte.MethodResults;

namespace Vkontakte
{
    /// <summary>
    /// Главный адаптер для вызова методов вконтакте.апи.
    /// Класс является синглтоном.
    /// </summary>
    public class VkAdapter:IVkAdapter
    {
        private SessionData SessionData;

        private static VkAdapter instance;

        public int UserId { get; set; }
        
        public int AppId { get; set; }

        public bool Authenticated { get; set; }

        public static VkAdapter Instance
        {
            get
            {
                return instance ?? (instance = new VkAdapter());
            }
        }
        
        private VkAdapter()
        {
            Authenticated = false;
        }

        /// <summary>
        /// Методо должен быть вызван перед вызовом любого другого метода API.
        /// </summary>
        /// <param name="sessionData"></param>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
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

        
        public T CallRemoteMethod<T>(string name, string version, Func<XElement, T> resultMethod, Dictionary<String, String> methodParams = null)
        {
            if(!Authenticated)
            {
                throw new SecurityException("User is not authenticated. Please, call Authenticate() method first.");
            }

            if(DateTime.Now > SessionData.SessionExpires)
            {
                throw new SessionExpiredException();
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
            catch (NullReferenceException)
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
                Enum.TryParse(error.TryGetElementValue("error_code"), out errorCode);
            }
            catch (NullReferenceException)
            {
                errorCode = ErrorCode.UnknownErrorOccured;
            }

            try
            {
                errorMessage = error.TryGetElementValue("error_msg");
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
