using System;
using AppYard.Lab.DataStore.InMemory;
using Humanizer;
using Manatee.Json.Schema;
using Microsoft.AspNetCore.Mvc;

namespace AppYard.Lab.DataManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemaController : ControllerBase
    {
        SchemaStore store = new SchemaStore();

        // GET: api/DataType
        [HttpGet(Name = nameof(GetAllSchema))]
        public IActionResult GetAllSchema()
        {
            return Ok(store.GetAll());
        }

        // GET: api/DataType/5
        [HttpGet("{schemaName}", Name = "GetSchema")]
        public IActionResult Get(string schemaName)
        {
            var item = store.Get(schemaName);
            if (item != null)
            {
                return Ok(store.Get(schemaName));
            }
            return NotFound();
        }

        // POST: api/DataType
        [HttpPost]
        public IActionResult Post([FromBody] JsonSchema schema)
        {
            if (schema is null)
            {
                return BadRequest("HTTP POST operation must contain valide a JSON Schema.");
            }

            var result = schema.ValidateSchema();
            if (!result.IsValid)
            {
                return BadRequest(result.MetaSchemaValidations);
            }
            if (!string.IsNullOrEmpty(schema.Id))
            {
                return BadRequest("JSON Schema cannot contain 'id' or '$id' attribute.");
            }

            var schemaTitle = schema.Title().Kebaberize();
            schema.Id($"https://localhost:5001/api/schema/{schemaTitle}");

            var idPropSchema = new JsonSchema() {
                new TypeKeyword(JsonSchemaType.String),
                new DescriptionKeyword("Unique identifier assigned to this entity")
            };

            schema.Property("_id", idPropSchema);
            schema.Required("_id");
            
            store.Add(schemaTitle, schema);
            return base.CreatedAtRoute("GetSchema", new { schemaName = schemaTitle }, schema);
        }

        // PUT: api/DataType/5
        [HttpPut("{schemaName}")]
        public IActionResult Put(string schemaName, [FromBody] JsonSchema schema)
        {
            if (schema is null)
            {
                return BadRequest();
            }
            var result = schema.ValidateSchema();
            if (!result.IsValid)
            {
                return BadRequest(result.MetaSchemaValidations);
            }

            var item = store.Get(schemaName);
            if (item != null)
            {
                store.Update(schemaName, schema);
                return NoContent();
            }
            store.Add(schemaName, schema);
            return CreatedAtRoute("GetSchema", new { schemaName }, schema);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{schemaName}")]
        public IActionResult Delete(string schemaName)
        {
            var item = store.Get(schemaName);
            if (item != null)
            {
                store.Remove(schemaName);
                return NoContent();
            }
            return NotFound();
        }
    }
}
