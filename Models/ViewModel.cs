using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace DojoSecrets2.Models
{

    public class ViewModel
    {
        public Secret Secrets {get; set;}
        public LoginCheck loginUser {get; set;}
        public Like Likes {get; set;}

        public User regUser {get; set;}

       
    }
    



}