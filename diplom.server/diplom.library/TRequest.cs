using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom.library
{
    public abstract class TRequest
    {
        protected string fsUserId;
        protected Dictionary<string,double> fpModelParamValues;

        #region Constructors
        /**<summary>Конструктор.</summary>**/
        public TRequest()
        {
            fsUserId = "";
            fpModelParamValues = new Dictionary<string, double>();
        }
        #endregion

        #region Properties
        /**<summary>Id пользователя.</summary>**/
        public abstract string sUserId
        {
            get;
            set;
        }
        /**<summary>Список параметров модели.</summary>**/
        public abstract Dictionary<string, double> pModelParamValues
        {
            get;
            set;
        }
        #endregion

    }
}
