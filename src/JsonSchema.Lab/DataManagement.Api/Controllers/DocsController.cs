using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AppYard.Lab.DataStore.InMemory;
using Manatee.Json;
using Manatee.Json.Schema;
using Manatee.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace AppYard.Lab.DataManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocsController : ControllerBase
    {
        SchemaStore schemaStore = new SchemaStore();
        DocsStore docsStore = new DocsStore();

        // GET: api/Data
        [HttpGet("{schemaName}")]
        public IActionResult Get(string schemaName)
        {
            var schema = schemaStore.Get(schemaName);
            if (schema is null)
            {
                return NotFound();
            }
            var value = docsStore.GetAll(schemaName);
            if (value is null) return Ok(new object[] { });
            else return Ok(value);
        }

        // GET: api/Data/5
        [HttpGet("{schemaName}/{docId}", Name = "GetDoc")]
        public IActionResult Get(string schemaName, string docId)
        {
            var schema = schemaStore.Get(schemaName) as JsonSchema;
            if (schema is null)
            {
                return NotFound();
            }
            var value = docsStore.Get(schemaName, docId);
            if (value is null) return NotFound();
            else return Ok(value);
        }

        // POST: api/Data
        [HttpPost("{schemaName}")]
        public IActionResult Post(string schemaName, [FromBody] dynamic value)
        {
            if (value is null)
            {
                return BadRequest();
            }
            string docId = value._id = Guid.NewGuid().ToString();

            var schema = schemaStore.Get(schemaName) as JsonSchema;
            if (schema is null)
            {
                return NotFound();
            }
            JsonSerializer serializer = new JsonSerializer();
            var jsonValue = serializer.Serialize(value);

            var result = schema.Validate(jsonValue);
            if (!result.IsValid)
            {
                return BadRequest(result.Flatten());
            }
            docsStore.Add(schemaName, docId, value);
            return CreatedAtRoute("GetDoc", new { schemaName, docId }, value);

        }

        // PUT: api/Data/5
        [HttpPut("{schemaName}/{docId}")]
        public IActionResult Put(string schemaName, string docId, [FromBody] object value)
        {
            if (value is null)
            {
                return BadRequest();
            }
            var schema = schemaStore.Get(schemaName) as JsonSchema;
            if (schema is null)
            {
                return NotFound();
            }
            JsonSerializer serializer = new JsonSerializer();
            var jsonValue = serializer.Serialize(value);

            var result = schema.Validate(jsonValue);
            if (!result.IsValid)
            {
                return BadRequest(result.NestedResults);
            }

            var item = docsStore.Get(schemaName, docId);
            if (item != null)
            {
                docsStore.Update(schemaName, docId, value);
                return NoContent();
            }
            docsStore.Add(schemaName, docId, value);
            return CreatedAtRoute("GetDoc", new { schemaName, docId }, value);

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{schemaName}/{docId}")]
        public IActionResult Delete(string schemaName, string docId)
        {
            var schema = schemaStore.Get(schemaName) as JsonSchema;
            if (schema is null)
            {
                return NotFound();
            }

            var item = docsStore.Get(schemaName, docId);
            if (item != null)
            {
                docsStore.Remove(schemaName, docId);
                return NoContent();
            }
            return NotFound();

        }
    }
}
