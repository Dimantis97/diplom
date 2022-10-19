using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom.library
{
    /**<summary>Объект для записи лога приложения.</summary>**/
    public class TLog
    {
        private string fsPath;
        private string fsMsg;
        private int fiRes;

        #region Constructors
        /**<summary>Конструктор.</summary>**/
        public TLog()
        {
            sPath = "";
        }
        /**<summary>Конструктор.</summary>
         *<param name="_sPath">Путь к файлу лога.</param>**/
        public TLog(string _sPath)
        {
            sPath = _sPath;
        }
        #endregion

        #region Methods
        /**<summary>Записать лог.</summary>
         * <param name="_iCode">Код события.</param>
         * <param name="_sMsg">Сообщение события.</param>**/
        public int WriteLog(int _iCode, string _sMsg)
        {
            try
            {
                if(!File.Exists(sPath))
                    File.Create(sPath);
                using (StreamWriter pWriter = new StreamWriter(sPath))
                {
                    pWriter.Write(DateTime.Now.ToString("hh:mm:ss yyyy.MM.dd")+" "+_iCode.ToString()+" : "+_sMsg);
                }
                return TConsts.I_OK;
            }
            catch(Exception E)
            {
                return E.HResult;
            }
        }
        #endregion

        #region Properties
        /**<summary>Путь к файлу лога.</summary>**/
        public string sPath
        {
            get { return fsPath;}
            set { fsPath = value;}
        }
        /**<summary>Строка сообщения лога.</summary>**/
        public string sMsg
        {
            get { return fsMsg;}
            set { fsMsg = value;}
        }
        /**<summary>Код события лога.</summary>**/
        public int iRes
        {
            get { return fiRes;}
            set { fiRes = value;}
        }
        #endregion
    }
}
