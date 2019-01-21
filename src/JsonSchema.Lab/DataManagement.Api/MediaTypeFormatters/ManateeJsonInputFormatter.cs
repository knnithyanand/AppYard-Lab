using Manatee.Json;
using Manatee.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AppYard.Lab.DataManagement.Api.MediaTypeFormatters
{
    public class ManateeJsonInputFormatter : TextInputFormatter
    {
        static JsonSerializer serializer = new JsonSerializer();

        public ManateeJsonInputFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue ("application/json"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        protected override bool CanReadType(Type type)
        {
            return (type != null) && base.CanReadType(type);
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var request = context.HttpContext.Request;
            var type = context.ModelType;
            object model;

            try
            {
                using (var reader = context.ReaderFactory(request.Body, encoding))
                {
                    var jsonValue = JsonValue.Parse(reader.ReadToEnd());
                    model = typeof(JsonValue) == type ? jsonValue : serializer.Deserialize(type, jsonValue);
                }
                if (model == null && !context.TreatEmptyInputAsDefaultValue)
                {
                    return InputFormatterResult.NoValueAsync();
                }
                else
                {
                    return InputFormatterResult.SuccessAsync(model);
                }
            }
            catch
            {
                return InputFormatterResult.FailureAsync();
            }
        }

    }
}
