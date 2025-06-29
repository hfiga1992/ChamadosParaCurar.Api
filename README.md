# ChamadosParaCurar.Api

## Configuração de Credenciais

Para executar este projeto localmente, você precisará configurar suas credenciais no arquivo `appsettings.json`. 

### MongoDB

1. No arquivo `appsettings.json`, substitua `<senha>` na string de conexão do MongoDB pela senha real:
   ```json
   "MongoDbSettings": {
     "ConnectionString": "mongodb+srv://heitoriga1992:<senha>@chamadosparacurar.f24mft5.mongodb.net/?retryWrites=true&w=majority&appName=ChamadosParaCurar",
     "DatabaseName": "ChamadosParaCurar",
     "UsuarioCollectionName": "usuarios",
     "DevocionalCollectionName": "devocionais"
   }
   ```

### SendGrid

1. No arquivo `appsettings.json`, substitua `<sendgrid-api-key>` pela chave API real do SendGrid:
   ```json
   "SendGridSettings": {
     "ApiKey": "<sendgrid-api-key>",
     "RemetenteEmail": "chamadosparacurar@gmail.com",
     "RemetenteNome": "Chamados Para Curar"
   }
   ```

### Segurança

- O arquivo `appsettings.json` está incluído no `.gitignore` para evitar o envio de credenciais para o repositório.
- Para ambientes de produção, recomenda-se o uso de variáveis de ambiente ou serviços de gerenciamento de segredos.

## Executando o Projeto

```bash
dotnet restore
dotnet run
```

O aplicativo estará disponível em: http://localhost:5002
