using Microsoft.AspNetCore.Mvc;
using RecipesApp.Models;
using System;
using System.Text.Json;
using System.Collections.Generic;

namespace RecipesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        string fileName = "Users.json";

        // GET api/users
        [HttpGet]
        public IEnumerable<User> Get()
        {
            List<User> users = ReadJsonuser();
            return users;
        }

        private List<User> ReadJsonuser()
        {
            string fileText = System.IO.File.ReadAllText(fileName);
            List<User>? users = null;
            try
            {
                users = JsonSerializer.Deserialize<List<User>>(fileText);
            }
            catch (Exception)
            {
                // ignore errors and return null list
            }
            return users;
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            List<User> users = ReadJsonuser();
            var found = users.Find(u => u.UserId == id);
            if (found == null)
                return NotFound();
            return found;
        }

        // POST api/users
        [HttpPost]
        public ActionResult<User> Post([FromBody] User user)
        {
            var users = ReadJsonuser();

            int nextId = users.Any() ? users.Max(u => u.UserId) + 1 : 1;
            user.UserId = nextId;
            users.Add(user);

            string stringJson = JsonSerializer.Serialize(users);
            System.IO.File.WriteAllText(fileName, stringJson);

            return CreatedAtAction(nameof(Get), new { id = user.UserId }, user);
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] User user)
        {
            List<User> users = ReadJsonuser();
            User found = users.Find(u => u.UserId == id);
            if (found == null)
                return NotFound();

            found.Name = user.Name;
            found.Surname = user.Surname;

            string json = JsonSerializer.Serialize(users);
            System.IO.File.WriteAllText(fileName, json);

            return Ok("Pomyślnie zaktualizowano");
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            List<User> users = ReadJsonuser();
            User found = users.Find(u => u.UserId == id);
            if (found == null)
                return NotFound();

            users.Remove(found);

            string json = JsonSerializer.Serialize(users);
            System.IO.File.WriteAllText(fileName, json);

            return Ok("Pomyślnie usunięto");
        }
    }
}