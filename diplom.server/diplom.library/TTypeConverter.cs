using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Security.Cryptography;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace diplom.library
{
    public static class TTypeConverter
    {
        private static Dictionary<Type,SqlDbType> fpSqlDbTypeDict;

        #region Constructors
        /**<summary>Конструктор.</summary>**/
        static TTypeConverter()
        {
            fpSqlDbTypeDict = new Dictionary<Type, SqlDbType>()
            {
                {typeof(bool),     SqlDbType.Bit},              //булева величина
                {typeof(Int16),    SqlDbType.SmallInt},         //целое 16-разрядное число
                {typeof(int),      SqlDbType.Int},              //целое 32-разрядное число
                {typeof(long),     SqlDbType.BigInt},           //целое 64-разрядное число
                {typeof(float),    SqlDbType.Real},             //десятичное число с плавающей запятой, единичная точность
                {typeof(double),   SqlDbType.Float},            //десятичное число с плавающей запятой, двойная точность
                {typeof(decimal),  SqlDbType.Decimal },         //десятичное число с плавающей запятой.
                {typeof(string),   SqlDbType.VarChar},          //строка 1:8000 символов
                {typeof(DateTime), SqlDbType.DateTime},         //дата
                {typeof(Guid),     SqlDbType.UniqueIdentifier}, //GUID
                {typeof(object),   SqlDbType.Variant },         //object
                {typeof(byte[]),   SqlDbType.Binary }           //бинарник
            };
        }
        #endregion

        #region Methods
        /**<summary>Получение значения SqlDbType соответствующий типу System.Type.</summary>
         * <param name="_tSqlType">Возвращаемый тип SqlDbType.</param>
         * <param name="_tType">Тип, по которому возвращается SqlDbType.</param>**/
        public static bool ConvertType(Type _tType, out SqlDbType _tSqlType)
        {
            return fpSqlDbTypeDict.TryGetValue(_tType, out _tSqlType);
        }
        /**<summary>Преобразование bool к double.</summary>
         * <param name="_bValue">Значение, преобразуемое к double.</param>**/
        public static double ConvertToDouble(bool _bValue)
        {
            if(_bValue)
                return (double)1;
            else 
                return (double)0;
        }
        /**<summary>Сериализация JObject.</summary>"/>
         * <param name="_pObj">JSON к сериализации.</param>**/
        public static byte[] SerializeJObject(JObject _pObj)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_pObj));
        }
        /**<summary>Получить hash строки.</summary>
         * <param name="_sInputString">Входящая строка.</param>
         * <returns>Хэш-строка.</returns>**/
        public static string GetHash(string _sInputString)
        {
            StringBuilder pStrBuilder = new StringBuilder();
            using (HashAlgorithm pAlg = SHA256.Create())
            {
                byte[] arHashBytes = pAlg.ComputeHash(Encoding.UTF8.GetBytes(_sInputString)); //получение хэша байтов по правилу SHA256
                //преобразование байтов к строке
                foreach (byte b in arHashBytes)
                    pStrBuilder.Append(b.ToString("x2"));
            }
            return pStrBuilder.ToString();
        }
        #endregion

    }
}
