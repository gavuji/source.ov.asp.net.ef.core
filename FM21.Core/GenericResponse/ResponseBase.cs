using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FM21.Core
{
    public abstract class ResponseBase
    {
        #region Private Variable
        private string msg;
        private Exception exception;
        #endregion
        protected ResponseBase(ResultType _ResultType)
        {
            Result = _ResultType;
        }

        #region Public Properties
        public string Message
        {
            get
            {
                // In case of error no need to set error msg 
                if (Exception != null && (string.IsNullOrEmpty(msg)))
                {
                    return Exception.Message;
                }
                return msg;
            }
            set => msg = value;
        }

        public Exception Exception
        {
            get => exception;
            set
            {
                exception = value;
                Result = ResultType.Error;
            }
        }

        public ResultType Result { get; set; }
        public List<KeyValuePair<string, string>> ExtraData { get; set; }
        #endregion

        #region Methods
        public void SetInfo(ValidationResult results)
        {
            ExtraData = new List<KeyValuePair<string, string>>();
            results.Errors.ToList()
                        .ForEach(err =>
                        {
                            ExtraData.Add(new KeyValuePair<string, string>(err.PropertyName, err.ErrorMessage));
                        });
        }
        #endregion
    }
}