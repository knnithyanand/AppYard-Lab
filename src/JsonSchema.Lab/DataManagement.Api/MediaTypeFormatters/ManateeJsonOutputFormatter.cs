using Manatee.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AppYard.Lab.DataManagement.Api.MediaTypeFormatters
{
    public class ManateeJsonOutputFormatter : TextOutputFormatter
    {
        static JsonSerializer serializer;

        static ManateeJsonOutputFormatter()
        {
            serializer = new JsonSerializer();
        }

        public ManateeJsonOutputFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            return (type != null) && base.CanWriteType(type);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (selectedEncoding is null)
            {
                throw new ArgumentNullException(nameof(selectedEncoding));
            }

            var response = context.HttpContext.Response;
            var type = context.ObjectType;
            var model = context.Object;

            using (var writer = context.WriterFactory(response.Body, selectedEncoding))
            {
                if (writer is null)
                {
                    throw new NullReferenceException(nameof(writer));
                }
                var jsonValue = serializer.Serialize(type, model);
                writer.Write(jsonValue);
                await writer.FlushAsync();
            }
        }
    }
}
