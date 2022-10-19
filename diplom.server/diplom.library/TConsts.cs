using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom.library
{
    /**<summary>Константы.</summary>**/
    public class TConsts                                     
    {
        #region Codes
        /**<summary>Код ошибки: все в порядке.</summary>**/
        public const int I_OK                               = 0;
        /**<summary>Код ошибки: ошибка при чтении параметров модели из csv-файла.</summary>**/
        public const int I_ERR_READ_CSV                     = 101;
        /**<summary>Код ошибки: ошибка при калькуляции модели - отсутствует константа.</summary>**/
        public const int I_ERR_CALC_MODEL_NO_CONST          = 1020;
        /**<summary>Код ошибки: ошибка при калькуляции модели - невозможно получить параметр по ключу.</summary>**/
        public const int I_ERR_CALC_MODEL_NO_PARAM          = 1021;
        /**<summary>Код ошибки: ошибка конфига - ошибка при чтении из XML.</summary>**/
        public const int I_ERR_CONFIG_LOAD_XML              = 1030;
        /**<summary>Код ошибки: ошибка при выполнении SQL-запроса - не совпадает количество параметров и имен параметров.</summary>**/
        public const int I_ERR_SQL_PARAMS_NAME_NUM          = 1040;
        /**<summary>Код ошибки: ошибка при выполнении SQL-запроса - не удалось ковертировать тип параметра.</summary>**/
        public const int I_ERR_SQL_PARAM_TYPE_CONV          = 1041;
        /**<summary>Код ошибки: ошибка при чтении данных запроса - несуществующий тип модели.</summary>**/
        public const int I_ERR_READING_REQ_DATA_MODEL       = 1050;
        /**<summary>Код ошибки: ошибка при чтении данных запроса - не найден объект medicalData.</summary>**/
        public const int I_ERR_READING_REQ_DATA_MED         = 1051;
        /**<summary>Код ошибки: ошибка при чтении данных запроса - не найден объект userData.</summary>**/
        public const int I_ERR_READING_REQ_DATA_USER        = 1052;
        /**<summary>Код ошибки: ошибка при чтении данных запроса - не найден объект ekgData.</summary>**/
        public const int I_ERR_READING_REQ_DATA_EKG         = 1053;
        /**<summary>Код ошибки: ошибка хэндлера - модель с указанным именем не найдена.</summary>**/
        public const int I_ERR_HANDL_RETRIEV_MODEL_BY_NAME  = 1060;

        #endregion

        #region Error strings
        /**<summary>Строка ошибки: ошибка при чтении коэффициентов и характеристик модели из csv-файла.</summary>**/
        public const string S_ERR_READ_CSV                     = "Ошибка при чтении коэффициентов и характеристик модели из csv-файла. Не удалось получить данные.";
        /**<summary>Строка ошибки: ошибка при калькуляции модели - отсутствует константа. </summary>**/
        public const string S_ERR_CALC_MODEL_NO_CONST          = "Ошибка при калькуляции модели - отсутствует константа.";
        /**<summary>Строка ошибки: ошибка при калькуляции модели - невозможно получить параметр по ключу.</summary>**/
        public const string S_ERR_CALC_MODEL_NO_PARAM          = "Ошибка при калькуляции модели - невозможно получить параметр по ключу.";
        /**<summary>Строка ошибки: ошибка при выполнении SQL-запроса - не совпадает количество параметров и имен параметров.</summary>**/
        public const string S_ERR_SQL_PARAMS_NAME_NUM          = "Ошибка при выполнении SQL-запроса - не совпадает количество параметров и имен параметров.";
        /**<summary>Строка ошибки: ошибка при выполнении SQL-запроса - не удалось ковертировать тип параметра.</summary>**/
        public const string S_ERR_SQL_PARAM_TYPE_CONV          = "Ошибка при выполнении SQL-запроса - не удалось ковертировать тип параметра.";
        /**<summary>Строка ошибки: ошибка при чтении данных запроса - несуществующий тип модели.</summary>**/
        public const string S_ERR_READING_REQ_DATA_MODEL       = "Ошибка при чтении данных запроса - несуществующий тип модели.";
        /**<summary>Строка ошибки: ошибка при чтении данных запроса - не найден объект medicalData.</summary>**/
        public const string S_ERR_READING_REQ_DATA_MED         = "Ошибка при чтении данных запроса - не найден объект medicalData.";
        /**<summary>Строка ошибки: ошибка при чтении данных запроса - не найден объект userData.</summary>**/
        public const string S_ERR_READING_REQ_DATA_USER        = "Ошибка при чтении данных запроса - не найден объект ekgData.";
        /**<summary>Строка ошибки: ошибка при чтении данных запроса - не найден объект userData.</summary>**/
        public const string S_ERR_READING_REQ_DATA_EKG         = "Ошибка при чтении данных запроса - не найден объект ekgData.";
        /**<summary>Строка ошибки: ошибка хэндлера - модель с указанным именем не найдена.</summary>**/
        public const string S_ERR_HANDL_RETRIEV_MODEL_BY_NAME  = "Ошибка хэндлера - модель с указанным именем не найдена.";
        #endregion

        #region Model types
        /**<summary>Тип модели: регрессионная логистическая модель.</summary>**/
        public const string S_MODEL_TYPE_LOG_REG = "LogReg";
        #endregion

        #region Other
        /**<summary>Прочее: Константа.</summary>**/
        public const string S_CONST                      = "Константа";
        /**<summary>Прочее: Путь к файлу конфига.</summary>**/
        public const string S_CONF_FILEPATH              = "./Config.xml";
        /**<summary>Прочее: XPath к узлу данных БД в конфиге.</summary>**/
        public const string S_CONF_DB_XPATH              = "//MainDB";
        /**<summary>Прочее: XPath к узлу данных моделей в конфиге.</summary>**/
        public const string S_MODEL_XPATH                = "//Models";
        /**<summary>Прочее: XPath к узлу данных параметров для http-подключения.</summary>**/
        public const string S_CONF_HTTP_PARAMS_XPATH     = "//HTTPParams";
        /**<summary>Прочее: имя аттрибута строки подключения к БД.</summary>**/
        public const string S_CONN_STR_ATTR_NAME         = "ConnStr";
        /**<summary>Прочее: имя аттрибута пути к файлу с коэффициентами уравнения.</summary>**/
        public const string S_FILEPATH_ATTR_NAME         = "sParamsFile";
        /**<summary>Прочее: имя аттрибута пути к файлу с характеристиками уравнения.</summary>**/
        public const string S_CHARACT_FILEPATH_ATTR_NAME = "sCharactFile";
        /**<summary>Прочее: имя аттрибута типа модели.</summary>**/
        public const string S_MODEL_TYPE_ATTR_NAME       = "sType";
        /**<summary>Прочее: имя аттрибута имени модели.</summary>**/
        public const string S_MODEL_MODEL_NAME_ATTR_NAME = "sName";
        /**<summary>Прочее: имя аттрибута http-префикса.</summary>**/
        public const string S_PREF_ATTR_NAME             = "sPath";
        /**<summary>Прочее: путь к файлу лога.</summary>**/
        public const string S_LOG_FILEPATH               = "./Log.txt";
        /**<summary>Прочее: разделитель в csv-файле.</summary>**/
        public const char   C_DELIM                      = ';';
        /**<summary>Прочее: паттерн @DATA</summary>**/
        public const string S_DATA_PATT                  = "@DATA";
        /**<summary>Прочее: чувствительность модели.</summary>**/
        public const string S_MODEL_SENS                 = "fModelSens";
        /**<summary>Прочее: точность модели.</summary>**/
        public const string S_MODEL_ACCUR                = "fModelAccur";
        /**<summary>Прочее: специфичность модели.</summary>**/
        public const string S_MODEL_SPEC                 = "fModelSpec";
        #endregion

        #region Console commands
        /**<summary>Консольная команда: закончить работу.</summary>**/
        public const string S_CONSOLE_COMM_ABORT = ":q";
        #endregion

        #region Logical Regression Model Aliases
        /**<summary>Псевдоним свойства JSON: модель.</summary>**/
        public const string S_JSON_ALIAS_MODEL      = "model";
        /**<summary>Псевдоним свойства JSON: id пользователя.</summary>**/
        public const string S_JSON_ALIAS_USER_ID    = "userId";
        /**<summary>Псевдоним свойства JSON: медицинские данные.</summary>**/
        public const string S_JSON_ALIAS_MED_DATA   = "medicalData";
        /**<summary>Псевдоним свойства JSON: личные данные пользователя.</summary>**/
        public const string S_JSON_ALIAS_USER_DATA  = "userData";
        /**<summary>Псевдоним свойства JSON: cardiostimulator.</summary>**/
        public const string S_JSON_ALIAS_STIMULATOR = "cardiostimulator";
        /**<summary>Псевдоним свойства JSON: smoking.</summary>**/
        public const string S_JSON_ALIAS_SMOKING    = "smoking";
        /**<summary>Псевдоним свойства JSON: diseasediabetes.</summary>**/
        public const string S_JSON_ALIAS_DIABETE    = "diseasediabetes";
        /**<summary>Псевдоним свойства JSON: diseasehypertonia.</summary>**/
        public const string S_JSON_ALIAS_HYPERTONIA = "diseasehypertonia";
        /**<summary>Псевдоним свойства JSON: age.</summary>**/
        public const string S_JSON_ALIAS_AGE        = "age";
        /**<summary>Псевдоним свойства JSON: id пользователя.</summary>**/
        public const string S_JSON_ALIAS_GENDER     = "gender";
        /**<summary>Псевдоним свойства JSON: weight.</summary>**/
        public const string S_JSON_ALIAS_WEIGHT     = "weight";
        /**<summary>Псевдоним свойства JSON: weight_.</summary>**/
        public const string S_JSON_ALIAS_WEIGHT_    = "weight_";
        /**<summary>Псевдоним свойства JSON: heght.</summary>**/
        public const string S_JSON_ALIAS_HEIGHT     = "height";
        /**<summary>Псевдоним свойства JSON: данные ЭКГ.</summary>**/
        public const string S_JSON_ALIAS_EKG        = "ekgData";
        #endregion

        #region Common JSON aliases
        /**<summary>Общий псевдоним JSON: iRes.</summary>**/
        public const string S_I_RES         = "iRes";
        /**<summary>Общий псевдоним JSON: sError.</summary>**/
        public const string S_ERROR         = "sError";
        /**<summary>Общий псевдоним JSON: sensetivity.</summary>**/
        public const string S_SENSITIVITY   = "sensetivity";
        /**<summary>Общий псевдоним JSON: specificity.</summary>**/
        public const string S_SPECIFICITY   = "specificity";
        /**<summary>Общий псевдоним JSON: accuracy.</summary>**/
        public const string S_ACCURACY      = "accuracy";
        /**<summary>Общий псевдоним JSON: calculateId.</summary>**/
        public const string S_CALCULATE_ID  = "calculateId";
        /**<summary>Общий псевдоним JSON: resultOfModel.</summary>**/
        public const string S_RES_OF_MODEL  = "resultOfModel";
        /**<summary>Общий псевдоним JSON: resultOfModel.</summary>**/
        public const string S_MODEL_CHARACT = "modelCharacteristics";
        #endregion

        #region Logical Regression Model Values
        /**<summary>Значения свойства: модель.</summary>**/
        public const string S_JSON_VALUE_MODEL_LOG_REG      = "logicalRegression";
        #endregion

        #region Sql
        /**<summary>Sql-команда: добавление данных в таблицу samples.</summary>**/
        public const string S_SQL_INSERT_SAMPLES           = "insert into samples ({0}) values ({1})";
        /**<summary>Sql-команда: получение данных из таблцы users.</summary>**/
        public const string S_SQL_SELECT_USERS             = "select * from users where external_id = @DATA0";
        /**<summary>Sql-команда: добавление нового пользователя в таблицу users.</summary>**/
        public const string S_SQL_INSERT_INTO_USERS        = "insert into users (internal_id,external_id) values (@DATA0, @DATA1)";

        /**<summary>Sql-паттерн: для вставки поля patient_id.</summary>**/
        public const string S_SQL_PATT_PATIENT_ID         = "patient_id,{0}";
        /**<summary>Sql-паттерн: для вставки поля {1} при конкатенации.</summary>**/
        public const string S_SQL_PATT_FIRST_PLACE_TO_FRM  = ",{1}";

        /**<summary>Sql-поле: internal_id.</summary>**/
        public const string S_SQL_INTERNAL_ID              = "internal_id";
        #endregion
    }
}
