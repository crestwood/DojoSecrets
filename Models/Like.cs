using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace DojoSecrets2.Models
{

    public class Like
    {
        [Key]
        public int Id {get;set;}
       
        public User Liker {get; set;}
        public Secret Secret {get;set;}
        public int UserId {get;set;}
        public int SecretId {get;set;}


    }
}