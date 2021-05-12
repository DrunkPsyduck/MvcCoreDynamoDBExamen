using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MvcCoreDynamoDBExamen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreDynamoDBExamen.Services
{
    public class ServiceAWSDynamoDB
    {
        private DynamoDBContext context;

        public ServiceAWSDynamoDB()
        {
            AmazonDynamoDBClient cliente = new AmazonDynamoDBClient();
            this.context = new DynamoDBContext(cliente);
        }

        public async Task CreateUsuario(Usuario user)
        {
            await this.context.SaveAsync<Usuario>(user);
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            var tabla = this.context.GetTargetTable<Usuario>();
            var scanOptions = new ScanOperationConfig();
            var results = tabla.Scan(scanOptions);
            List<Document> data = await results.GetNextSetAsync();
            IEnumerable<Usuario> users = this.context.FromDocuments<Usuario>(data);
            return users.ToList();
        }

        public async Task<Usuario> FindUser(int idusuario)
        {
            return await this.context.LoadAsync<Usuario>(idusuario);
        }

        public async Task ModifyUsuario(Usuario usuario)
        {
            Usuario user = await FindUser(usuario.IdUsuario);
            user.Nombre = usuario.Nombre;
            user.Descripcion = usuario.Descripcion;

            await this.context.SaveAsync<Usuario>(user);
        }

        public async Task EliminarUsuario(int idusuario)
        {
            await this.context.DeleteAsync<Usuario>(idusuario);
        }

    }
}
