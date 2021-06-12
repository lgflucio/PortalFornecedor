using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.ComponentModel;
using System.Linq;

namespace ExceptionHandler.Helpers
{
    public static class ExtensionMethodHelpers
    {
        /// <summary>
        /// Converter o objeto atual para JSON.
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="myObject"></param>
        /// <returns></returns>
        public static string ToJson<TType>(this TType myObject)
        {
            return JsonConvert.SerializeObject(myObject, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        /// <summary>
        /// Obter o valor da Annotation Description: [Description("Minha Descrição")]
        /// </summary>
        /// <typeparam name="TType">automaticamente obtido com o objeto.</typeparam>
        /// <param name="myEnum">objeto do tipo Enum.</param>
        /// <returns></returns>

        public static string GetDescription<TType>(this TType myEnum)
        {
            var _fieldInfo = myEnum.GetType().GetField(myEnum.ToString());

            var _customAttributes = (DescriptionAttribute[])_fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            return
                   _customAttributes != null && _customAttributes.Any()
                       ? _customAttributes[0].Description
                       : myEnum.ToString();
        }

        /// <summary>
        /// Obter os erros do ModelState separados por ponto e vírgula.
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static string GetAllErrorsToString(this ModelStateDictionary modelState)
        {
            var _errors = modelState?.Values.SelectMany(x => x?.Errors)
                            ?.Select(x => x?.ErrorMessage);

            return _errors != null && _errors.Any()
                        ? string.Join(" ", _errors)
                        : "Verifique os dados inseridos.";
        }

        public static bool TryDeserializeJson<TType>(this object objToConvert, out TType resultParsed)
        {
            resultParsed = default(TType);

            try
            {
                if (!(objToConvert is string))
                    objToConvert = objToConvert.ToJson();

                resultParsed = JsonConvert.DeserializeObject<TType>(objToConvert as string);

                return resultParsed != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}