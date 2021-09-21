using System;

namespace SecKill.Model
{
    public class BusinessException : SystemException
    {
        public string Code { get; set; }
        public string Msg { get; set; }

        public BusinessException(string code, string msg)
        {
            Code = code;
            Msg = msg;
        }
    }
}
