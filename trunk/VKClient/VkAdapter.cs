using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Vkontakte.MethodResults;

namespace Vkontakte
{
    public class VkAdapter:IVkAdapter
    {
        private SessionData SessionData;

        public int UserId { get; set; }
        public int AppId { get; set; }

        public VkAdapter(SessionData sessionData, int userId, int appId)
        {
            SessionData = sessionData;
            UserId = userId;
            AppId = appId;
        }

        // TODO: Check session expiration time before calling the method
        public IMethodResult CallRemoteMethod(string name, string version, Dictionary<String, String> methodParams, Func<XElement, IMethodResult> resultMethod)
        {
            string sig = Utils.MakeMethodSig(this.UserId, this.AppId, name, this.SessionData.SessionId, this.SessionData.SecretKey, methodParams);
            string url = Utils.GetRequestUrl(sig, this.SessionData.SessionId, methodParams);
            XElement root = XElement.Load(url);
            IMethodResult result = ParseMethodResults(root, resultMethod);
            return result;
        }

        public void CallRemoteMethodAsync()
        {
            throw new NotImplementedException();
        }

        protected IMethodResult ParseMethodResults(XElement response, Func<XElement, IMethodResult> parseMethod)
        {
            IMethodResult result;

            if (response.Name.LocalName == "error")
            {
                result = ParseErrorResult(response);
                return result;
            }

            try
            {
                result = parseMethod(response);
                return result;
            }
            catch (System.NullReferenceException ex)
            {

                return new ErrorResult()
                {
                    ErrorCode = ErrorCode.ParsingOfResultFailed,
                    ErrorMessage = Messages.ErrorMessages[ErrorCode.ParsingOfResultFailed]
                };
            }
        }

        protected ErrorResult ParseErrorResult(XElement error)
        {
            ErrorCode errorCode = new ErrorCode();
            Enum.TryParse(error.Element("error_code").Value, out errorCode);

            var result = new ErrorResult() { ErrorCode = errorCode, ErrorMessage = error.Element("error_msg").Value };
            return result;
        }
    }
}
