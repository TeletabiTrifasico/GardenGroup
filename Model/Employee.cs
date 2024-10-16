using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Model
{
    public enum Privilieges
    {
        NormalUser,
        ServiceDesk,
    }

    [DataObject]
    public class Employee
    {
        public ObjectId Id { get; set; }
        
        [BsonElement("first_name")]
        public string Firstname { get; set; } = string.Empty;
        
        [BsonElement("last_name")]
        public string Lastname { get; set; } = string.Empty;
        
        public string FullName => $"{Firstname} {Lastname}";
        
        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;
        
        [BsonElement("phone")]
        public string Phone { get; set; } = string.Empty;
        
        [BsonElement("user_type")] 
        public Privilieges UserType;
        
        [BsonElement("password")]
        public string Password { get; set; } = string.Empty;
        
        [BsonElement("username")]
        public string Username { get; set; } = string.Empty;
    }
}
