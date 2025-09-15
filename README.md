# BigBrain

Este repositório apresenta minha solução desenvolvida em .NET 8, cujo objetivo é realizar uma integração com o Microsoft Graph API para consulta de usuários e eventos de calendário de um tenant Azure AD.

## Como executar o projeto
Para executar o projeto, siga as seguintes etapas:
1. Clone este repositório em sua máquina local usando o comando:

   ```bash
   git clone https://github.com/ezequiel-lima/bigbrain-desafio.git
   ```
   
2. Abra o projeto no Visual Studio ou em outra IDE de sua preferência.
3. Configure a string de conexão do banco de dados no arquivo `appsettings.json`.
4. Adicione no Gerenciador de Segredos do Usuário (dotnet user-secrets) a seguinte configuração:
   
  ```json
  {
    "AzureAd": {
      "TenantId": "SEU_TENANT_ID",
      "ClientId": "SEU_CLIENT_ID",
      "ClientSecret": "SEU_CLIENT_SECRET"
    }
  }
  ```
    
5. Crie um **Banco de Dados** com o nome configurado em `DefaultConnection` e em `HangfireConnection`.
> ⚠️ **Atenção:** Essa etapa é importante para a criação da migração e das tabelas do Hangfire.

6. No Console do Gerenciador de Pacotes, execute o comando `Update-Database` para criar suas tabelas.  
7. Compile o projeto e execute a aplicação, Use o Hangfire para testar as rotinas.
