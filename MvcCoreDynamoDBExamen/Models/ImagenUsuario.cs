using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreDynamoDBExamen.Models
{
    public class ImagenUsuario
    {
        [DynamoDBProperty("tiutlo")]
        public String Titulo { get; set; }
        [DynamoDBProperty("imagen")]
        public int Imagen { get; set; }

    }
}
