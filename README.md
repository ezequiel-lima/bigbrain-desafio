# BigBrain

Este repositório apresenta minha solução desenvolvida em .NET 8, cujo objetivo é realizar uma integração com o Microsoft Graph API para consulta de usuários e eventos de calendário de um tenant Azure AD.

A aplicação realiza:

- Autenticação com o Microsoft Graph;
- Listagem de todos os usuários do tenant;
- Consulta dos eventos de calendário de cada usuário;
- Persistência dos dados no banco de dados SqlServer.

Além disso, foram aplicadas as seguintes práticas e ferramentas:

- **Agendamento de tarefas com Hangfire**, para execução automática das rotinas;
- **Testes unitários** utilizando xUnit e Moq;
- **Centralização das dependências NuGet** utilizando `Directory.Packages.props`;

## Demonstração

<img width="1904" height="827" alt="image" src="https://github.com/user-attachments/assets/054ec4e1-9d52-40d6-b02e-e5213fe848f8" />
<img width="1894" height="946" alt="image" src="https://github.com/user-attachments/assets/ef45caea-9b1f-4e11-9092-b4b45fa13571" />

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
